using Microsoft.EntityFrameworkCore;

namespace MsStore.Stocks.Api.Domain;

[Index(nameof(ProductIdRef))]
public class ReservationLine
{
    public int Id { get; set; }
    public Guid UserReservationId { get; set; }
    public UserReservation UserReservation { get; set; } = null!;
    public int ProductIdRef { get; set; }

    // How much was taken from this batch, so a release can put exactly that back
    public int Quantity { get; set; }
}
