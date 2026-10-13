using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.Domain;

[Index(nameof(ReservedByUserId))]
public class UserReservation
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(64)]
    public required string Category { get; set; }
    public required int ReservedByUserId { get; set; }

    [Column(TypeName = "nvarchar(16)")]
    public ReservationStatus Status { get; set; } = ReservationStatus.Active;

    // public List<ReservationLine> Lines { get; set; } = [];
}
