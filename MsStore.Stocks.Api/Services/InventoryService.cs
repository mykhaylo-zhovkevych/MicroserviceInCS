using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public class InventoryService
{

    public Task<CreatedResult> CreateBatchAsync(int productId, string category, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task<List<BatchStock>> GetBatchesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BatchStock?> GetBatchAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<DeletedResult> DeleteBatchAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<TakeResult> TakeBatchAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GiveBackAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }

    // Reservations
    public Task<(CreatedReserveResult Result, ReservationResponse? Reservation)> ReserveBatchAsync(string category, int quantity, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<ReleaseResult> ReservationReleaseAsync(Guid id)
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
