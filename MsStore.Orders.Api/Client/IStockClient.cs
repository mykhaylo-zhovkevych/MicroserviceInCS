using MsStore.Orders.Api.Client.Results;

namespace MsStore.Orders.Api.Client;

public interface IStockClient
{
    Task<bool> CreateBatchAsync(int productId, string category, int quantity);
    Task<bool> ReturnBatchAsync(int productId, int quantity);
    
    Task<StockBatch?> GetBatchAsync(int productId);
    Task<List<StockBatch>?> GetBatchesAsync();
    
    Task<TakeStockResult> TakeBatchAsync(int productId, int quantity);
    Task<DeleteStockResult> DeleteBatchAsync(int productId);
}