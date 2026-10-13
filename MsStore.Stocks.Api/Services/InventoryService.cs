using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.Domain.Enums;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Mapping;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public class InventoryService(StockDbContext db) : IInventoryService
{

    public async Task<CreatedBatchResult> CreateBatchAsync(int productId, string category, int quantity)
    {
        if (await db.Batches.AnyAsync(b => b.ProductId == productId))
        {
            return CreatedBatchResult.AlreadyExists;
        }

        db.Batches.Add(new Batch { ProductId = productId, Category = category, InStock = (uint)quantity, Capacity = (uint)quantity });
        await db.SaveChangesAsync();
        return CreatedBatchResult.Created;
    }

    public async Task<List<BatchStock>> GetBatchesAsync()
    {
        return await db.Batches
            .OrderBy(b => b.ProductId)
            .ProjectToStock()
            .ToListAsync();
    }

    public async Task<BatchStock?> GetBatchAsync(int productId)
    {
        return await db.Batches
            .Where(b => b.ProductId == productId)
            .ProjectToStock()
            .FirstOrDefaultAsync();
    }

    public async Task<DeletedBatchResult> DeleteBatchAsync(int productId)
    {
        Batch? batch = await db.Batches.FindAsync(productId);
        if (batch is null)
        {
            return DeletedBatchResult.NotFound;
        }

        bool hasActiveReservation = await db.ReservationLines.AnyAsync(l =>
            l.ProductIdRef == productId && l.UserReservation.Status == ReservationStatus.Active);
        if (hasActiveReservation)
        {
            return DeletedBatchResult.HasActiveReservations;
        }

        db.Batches.Remove(batch);
        await db.SaveChangesAsync();
        return DeletedBatchResult.Deleted;
    }

    public async Task<TakeBatchResult> TakeBatchAsync(int productId, int quantity)
    {
        Batch? batch = await db.Batches.FindAsync(productId);
        if (batch is null)
        {
            return TakeBatchResult.NotFound;
        }

        if (batch.InStock < quantity)
        {
            return TakeBatchResult.NotEnough;
        }

        batch.InStock -= (uint)quantity;
        await db.SaveChangesAsync();
        return TakeBatchResult.Taken;
    }

    public async Task<ReturnBatchResult> ReturnBatchAsync(int productId, int quantity)
    {
        Batch? batch = await db.Batches.FindAsync(productId);
        if (batch is null)
        {
            return ReturnBatchResult.NotFound;
        }

        // Reserved units are still part of the batch, so they count towards the capacity
        int reserved = await db.ReservationLines
            .Where(l => l.ProductIdRef == productId && l.UserReservation.Status == ReservationStatus.Active)
            .SumAsync(l => l.Quantity);

        if (batch.InStock + reserved + quantity > batch.Capacity)
        {
            return ReturnBatchResult.ExceedsCapacity;
        }

        batch.InStock += (uint)quantity;
        await db.SaveChangesAsync();
        return ReturnBatchResult.Returned;
    }
}
