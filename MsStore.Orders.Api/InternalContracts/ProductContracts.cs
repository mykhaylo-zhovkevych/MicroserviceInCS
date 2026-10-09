using System.ComponentModel.DataAnnotations;

namespace MsStore.Orders.Api.InternalContracts;

public record CreateProductRequest(
    [Required, MaxLength(24)] string Category,
    [Required] string Name,
    [Range(0, double.MaxValue)] decimal Price,
    [Required] string Description,
    [Range(1, int.MaxValue)] int InitialQuantity);

public record UpdateProductRequest(
    [Required] string Name,
    [Range(0, double.MaxValue)] decimal Price,
    [Required] string Description);

public record ProductStyleResponse(
    int Id,
    string Category,
    string Name,
    string Description,
    decimal Price,
    int? OnHand,
    int? Reserved,
    int? Available);

