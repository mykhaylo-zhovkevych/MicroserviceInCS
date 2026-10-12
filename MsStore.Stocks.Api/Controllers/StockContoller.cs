using Microsoft.AspNetCore.Mvc;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services;
using MsStore.Stocks.Api.Services.Results;
using CreatedResult = MsStore.Stocks.Api.Services.Results.CreatedResult;

namespace MsStore.Stocks.Api.Controllers;

[ApiController]
[Route ("api/[controller]")]
public class StockContoller(InventoryService inventoryService) : ControllerBase
{

    [HttpPost("batches")]
    public async Task<IActionResult> CreateBatch(CreateBatchRequest req)
    {
        CreatedResult result = await inventoryService.CreateBatchAsync(req.ProductId, req.Category, req.Quantity);
        if (result == CreatedResult.AlreadyExists)
        {
            return Conflict($"Batch for product {req.ProductId} already exists, quantity can be added only once");
        }

        BatchStock? created = await inventoryService.GetBatchAsync(req.ProductId);
        return CreatedAtAction(nameof(GetBatch), new { productId = req.ProductId }, created);
    }

    [HttpGet("batches")]
    public async Task<ActionResult<List<BatchStock>>> GetBatches()
    {
        List<BatchStock> batches = await inventoryService.GetBatchesAsync();
        return batches is null ? NotFound() : batches;
    }
    
    [HttpGet("batches/{productId:int}")]
    public async Task<ActionResult<BatchStock>> GetBatch(int productId)
    {
        BatchStock? batch = await inventoryService.GetBatchAsync(productId);
        return batch is null ? NotFound() : batch;
    }
}
