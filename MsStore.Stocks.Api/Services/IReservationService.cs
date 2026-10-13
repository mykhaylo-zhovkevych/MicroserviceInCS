using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public interface IReservationService
{
    Task<(CreatedReserveResult Result, ReservationResponse? Reservation)> ReserveBatchAsync(int userId, string category, int quantity);
    Task<ReleaseReservationResult> ReservationReleaseAsync(Guid id);

    Task<List<ReservationResponse>> GetReservationsAsync(int? userId, string? category);
    Task<ReservationResponse?> GetReservationAsync(Guid id);
}
