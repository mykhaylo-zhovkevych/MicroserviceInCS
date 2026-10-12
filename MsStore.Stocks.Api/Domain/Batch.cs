using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.Domain;

[Index(nameof(Category))]
public class Batch
{
    // the Id comes from Orders.Api, so no IDENTITY column here
    // without DatabaseGeneratedOption.None (= ValueGeneratedNever) an int key becomes IDENTITY by convention,
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required int ProductId { get; set; }

    [MaxLength(64)]
    public required string Category { get; set; }
    // A string column type makes EF store the enum as "OnHold" instead of 0..3
    [Column(TypeName = "nvarchar(20)")]
    public BatchStatus Status { get; set; } = BatchStatus.Available;
    public uint OnHand { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpectedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
