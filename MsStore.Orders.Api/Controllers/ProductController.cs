using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsStore.Orders.Api.Client;
using MsStore.Orders.Api.Data;
using MsStore.Orders.Api.Domain;
using MsStore.Orders.Api.Enum;
using MsStore.Orders.Api.InternalContracts;

namespace MsStore.Orders.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
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
        Product product = new Product
        {
            Category = req.Category,
            Name = req.Name,
            Description = req.Description,
            Price = req.Price
        };
        
        //1. save the product to db
        db.Products.Add(product);
        await db.SaveChangesAsync();
        
        // 2. request from another service a new product
        bool createdProduct = await stockClient.CreateBatchAsync(product.Id, product.Category, req.InitialQuantity);
        if (!createdProduct)
        {
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Stock Api did not created the batch, thereafter product has not been created.");
        }
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, ToStyleResponse(product, await stockClient.GetBatchAsync(product.Id)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest req)
    {
        Product? existingProduct = await db.Products.FindAsync(id);
        if (existingProduct is null)
        {
            return NotFound();
        }
        
        existingProduct.Name = req.Name;
        existingProduct.Description = req.Description;
        existingProduct.Price = req.Price;
        await db.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        Product? existingProduct = await db.Products.FindAsync(id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        switch (await stockClient.DeleteFromBatchAsync(id))
        {
            case DeleteStockResult.HasActiveReservation:
                return Conflict("Product has active reservers.");
            case DeleteStockResult.Unavailable:
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Stock API is not reachable.");
        }

        db.Products.Remove(existingProduct);
        await db.SaveChangesAsync();

        return NoContent();
    }
    

    private static ProductStyleResponse ToStyleResponse(Product product, StockBatch? stock) 
        => new (product.Id, product.Category, product.Name, product.Description, product.Price, stock.OnHand, stock.Reserved, stock.Available);
    
}
