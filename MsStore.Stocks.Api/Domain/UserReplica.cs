using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MsStore.Stocks.Api.Domain.Enums;

namespace MsStore.Stocks.Api.Domain;

public class UserReplica
{
    // the Id comes from Users.Api, so no IDENTITY column here
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [MaxLength(64)]
    public required string FullName { get; set; }

    [MaxLength(128)]
    public string? Email { get; set; }

    [Column(TypeName = "nvarchar(16)")]
    public UserType Type { get; set; }
    
}
