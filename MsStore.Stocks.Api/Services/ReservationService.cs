using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.Domain.Enums;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Mapping;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public class ReservationService(StockDbContext db, IUserSyncService userSyncService) : IReservationService
{
    public async Task<(CreatedReserveResult Result, ReservationResponse? Reservation)> ReserveBatchAsync(int userId, string category, int quantity)
    {
        if (await db.Users.AnyAsync(u => u.Id == userId))
        {
            // Unknown locally, the replica may just be behind Users.Api
            if (await userSyncService.SyncAllAsync() is null)
            {
                return (CreatedReserveResult.UsersApiUnavailable, null);
            }
            if (await db.Users.AnyAsync(u => u.Id == userId))
            {
                return (CreatedReserveResult.NotFound, null);
            }
        }

        List<Batch> batches = await db.Batches
            .Where(b => b.Category == category && b.Status == BatchStatus.Available)
            .OrderBy(b => b.CreatedAt)
            .ToListAsync();

        if (batches.Sum(b => b.InStock) < quantity)
        {
            return (CreatedReserveResult.NotEnough, null);
        }
        
        UserReservation reservation = new UserReservation
            { Id = Guid.NewGuid(), Category = category, ReservedByUserId = userId };
        db.Reservations.Add(reservation);
        
        // Look for each Batch capacity and resaves, until it satisfied
        int remaining =  quantity;
        foreach (Batch batch in batches)
        {
            int taken = (int)Math.Min(batch.InStock, (uint)remaining);
            batch.InStock -= (uint)taken;
            
            db.ReservationLines.Add(new ReservationLine {UserReservationId = reservation.Id, ProductIdRef = batch.ProductId, Quantity = taken});

            remaining -= taken;
            if (remaining == 0)
            {
                break;
            }
        }

        await db.SaveChangesAsync();
        return (CreatedReserveResult.Reserved, reservation.ToResponse());
    }

    public async Task<ReleaseReservationResult> ReservationReleaseAsync(Guid id)
    {
        UserReservation? reservation = await db.Reservations.FindAsync(id);
        if (reservation is null)
        {
            return ReleaseReservationResult.NotFound;
        }
        if (reservation.Status == ReservationStatus.Released)
        {
            return ReleaseReservationResult.AlreadyReleased;
        }

        List<ReservationLine> lines = await db.ReservationLines
            .Where(l => l.UserReservationId == id)
            .ToListAsync();

        List<int> productIds = lines.Select(l => l.ProductIdRef).ToList();
        
        Dictionary<int, Batch> batches = await db.Batches
            .Where(b => productIds.Contains(b.ProductId))
            .ToDictionaryAsync(b => b.ProductId);

        foreach (ReservationLine line in lines)
        {
            if (batches.TryGetValue(line.ProductIdRef, out Batch? batch))
            {
                batch.InStock += (uint)line.Quantity;
            }
        }

        reservation.Status = ReservationStatus.Released;
        await db.SaveChangesAsync();
        return ReleaseReservationResult.Released;
    }

    public async Task<List<ReservationResponse>> GetReservationsAsync(int? userId, string? category)
    {
        IQueryable<UserReservation> query = db.Reservations;
        if (userId is not null)
        {
            query = query.Where(r => r.ReservedByUserId == userId);
        }
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(r => r.Category == category);
        }

        return await query
            .ProjectToResponse()
            .ToListAsync();
    }

    public async Task<ReservationResponse?> GetReservationAsync(Guid id)
    {
        return await db.Reservations
            .Where(r => r.Id == id)
            .ProjectToResponse()
            .FirstOrDefaultAsync();
    }
}
