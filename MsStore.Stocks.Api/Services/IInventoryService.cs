using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public interface IInventoryService
{
    Task<CreatedBatchResult> CreateBatchAsync(int productId, string category, int quantity);

    Task<List<BatchStock>> GetBatchesAsync();
    Task<BatchStock?> GetBatchAsync(int productId);

    Task<DeletedBatchResult> DeleteBatchAsync(int productId);
    Task<TakeBatchResult> TakeBatchAsync(int productId, int quantity);
    Task<ReturnBatchResult> ReturnBatchAsync(int productId, int quantity);
}
