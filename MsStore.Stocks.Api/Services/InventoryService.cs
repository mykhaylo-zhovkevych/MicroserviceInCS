using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Mapping;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public class InventoryService(StockDbContext db) : IInventoryService
{

    public async Task<CreatedBatchResult> CreateBatchAsync(int productId, string category, int quantity)
    {
        if (await db.Batches.AnyAsync(b => b.ProductId == productId))
        {
            return CreatedBatchResult.AlreadyExists;
        }

        db.Batches.Add(new Batch { ProductId = productId, Category = category, InStock = (uint)quantity });
        await db.SaveChangesAsync();
        return CreatedBatchResult.Created;
    }

    public async Task<List<BatchStock>> GetBatchesAsync()
    {
        return await db.Batches
            .OrderBy(b => b.ProductId)
            .ProjectToStock()
            .ToListAsync();
    }

    public Task<BatchStock?> GetBatchAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<DeletedBatchResult> DeleteBatchAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<TakeBatchResult> TakeBatchAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReturnBatchAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }
}
