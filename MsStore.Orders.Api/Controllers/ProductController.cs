using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsStore.Orders.Api.Client;
using MsStore.Orders.Api.Data;
using MsStore.Orders.Api.Domain;
using MsStore.Orders.Api.InternalContracts;

namespace MsStore.Orders.Api.Controllers;


[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class ProductController(OrdersDbContext db, IStockClient stockClient) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductStyleResponse>>> GetProducts()
    {
        var products = await db.Products.ToListAsync();
        Dictionary<int, StockBatch> batches = (await stockClient.GetBatchesAsync())?.ToDictionary(b => b.ProductId);
        
        return products.Select(p => ToStyleResponse(p, batches?.GetValueOrDefault(p.Id))).ToList();

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductStyleResponse>> GetProduct(int id)
    {
        var products = await db.Products.FindAsync(id);
        if (products == null)
        {
            return NotFound();
        }

        return ToStyleResponse(products, await stockClient.GetBatchAsync(id));

    }

    [HttpPost]
    public async Task<ActionResult<ProductStyleResponse>> CreateProduct(CreateProductRequest req)
    {
        return null;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest req)
    {
        return null;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        return null;
    }
    

    private static ProductStyleResponse ToStyleResponse(Product product, StockBatch? stock) 
        => new (product.Id, product.Category, product.Name, product.Description, product.Price, stock.OnHand, stock.Reserved, stock.Available);
    
}
