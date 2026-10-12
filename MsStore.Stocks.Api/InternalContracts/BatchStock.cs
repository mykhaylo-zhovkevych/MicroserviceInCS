using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.InternalContracts;

public record BatchStock(int ProductId, string Category, int Available) : BatchBase(Category, Available);


public record CreateBatchRequest(int ProductId, string Category, int Quantity);
public record CreateReservationRequest(int UserId, string Category, int Quantity);


public record ReservationResponse(Guid Id, string Category, int ReservedByUserId, ReservationStatus Status);
