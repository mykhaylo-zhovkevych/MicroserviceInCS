using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public class ReservationService(StockDbContext db, IUserSyncService userSyncService) : IReservationService
{
    public Task<(CreatedReserveResult Result, ReservationResponse? Reservation)> ReserveBatchAsync(string category, int quantity, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<ReleaseReservationResult> ReservationReleaseAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ReservationResponse>> GetReservationsAsync(int? userId, string? category)
    {
        throw new NotImplementedException();
    }

    public Task<ReservationResponse?> GetReservationAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}