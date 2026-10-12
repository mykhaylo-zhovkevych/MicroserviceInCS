using MsStore.Orders.Api.Client.Results;

namespace MsStore.Orders.Api.Client;

public interface IStockClient
{
    Task<bool> CreateBatchAsync(int productId, string category, int quantity);
    Task<bool> GiveBackAsync(int productId, int quantity);
    
    Task<StockBatch?> GetBatchAsync(int productId);
    Task<List<StockBatch>?> GetBatchesAsync();
    
    Task<TakeStockResult> TakeFromBatchAsync(int productId, int quantity);
    Task<DeleteStockResult> DeleteFromBatchAsync(int productId);
}