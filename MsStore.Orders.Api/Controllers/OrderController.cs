using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsStore.Orders.Api.Client;
using MsStore.Orders.Api.Data;
using MsStore.Orders.Api.Domain;
using MsStore.Orders.Api.Enum;
using MsStore.Orders.Api.InternalContracts;

namespace MsStore.Orders.Api.Controllers;


[Controller]
[Route("api/[controller]")]
public class OrderController(OrdersDbContext db, IStockClient stockClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await db.Orders.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(int productId, int quantity)
    {
        if (quantity <= 0)
        {
            return BadRequest("Quantity must be positive");
        }
        
        // price comes from my own database
        Product? product = await db.Products.FindAsync(productId);
        if (product is null)
        {
            return NotFound($"Product {productId} does not exist");
        }

        switch (await stockClient.TakeFromBatchAsync(productId, quantity))
        {
            case TakeStockResult.NotEnough:
                return Conflict("Not enough available stock (reserved units cannot be ordered)");
            case TakeStockResult.NotFound:
                return NotFound($"Product {productId} has no batch in Stock.Api");
            case TakeStockResult.Unavailable:
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Stock.Api is not reachable");
        }

        Order order = new Order()
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            AppliedPrice = product.Price,
            Quantity = quantity
        };

        try
        {
            db.Orders.Add(order);
            await db.SaveChangesAsync();
        }
        catch
        {
            await stockClient.GiveBackAsync(productId, quantity);
            throw;
        }

        return Ok(order);
    }
}
