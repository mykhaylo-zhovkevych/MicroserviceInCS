using System.ComponentModel.DataAnnotations;
using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.InternalContracts;

public record BatchStock(int ProductId, string Category, int Available) : BatchBase(Category, Available);


public record CreateBatchRequest(int ProductId, [Required, MaxLength(64)] string Category, [Range(1, int.MaxValue)] int Quantity);
public record CreateReservationRequest(int UserId, [Required, MaxLength(64)] string Category, [Range(1, int.MaxValue)] int Quantity);


public record ReservationResponse(Guid Id, string Category, int ReservedByUserId, ReservationStatus Status);
