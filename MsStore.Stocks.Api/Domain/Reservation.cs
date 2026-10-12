using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.Domain;

[Index(nameof(ReservedByUserId))]
public class Reservation
{
    public int Id { get; set; }

    [MaxLength(64)]
    public required string Category { get; set; }

    // The Id comes from Users.Api
    public int ReservedByUserId { get; set; }

    [Column(TypeName = "nvarchar(16)")]
    public ReservationStatus Status { get; set; } = ReservationStatus.Active;

    // public List<ReservationLine> Lines { get; set; } = [];
}
