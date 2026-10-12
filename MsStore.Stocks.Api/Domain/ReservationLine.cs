using Microsoft.EntityFrameworkCore;

namespace MsStore.Stocks.Api.Domain;

[Index(nameof(ProductIdRef))]
public class ReservationLine
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    // the Id comes from Orders.Api
    public int ProductIdRef { get; set; }
}
