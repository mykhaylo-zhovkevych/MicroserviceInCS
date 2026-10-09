namespace MsStore.Orders.Api.Domain;

public class Order
{
    // Should the setter be restricted
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Status { get; set; } = "Pending";
    public required int ProductId { get; set; }
    public required decimal AppliedPrice { get; set; }
    public required int Quantity { get; set; }
    public decimal TotalPrice => AppliedPrice * Quantity;
}
