using System.Net;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using MsStore.Orders.Api.Enum;

namespace MsStore.Orders.Api.Client;

// Represents an intermediate exchange value
public record StockBatch(int ProductId, string Sku, int OnHand, int Reserved, int Available);


public class StockClient(HttpClient http) : IStockClient
{
    public async Task<bool> CreateBatchAsync(int productId, string category, int qunatity)
    {
        try
        {
            HttpResponseMessage res = await http.PostAsJsonAsync("api/stock/batches", new { productId, category, qunatity });
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
    
    public async Task<bool> GiveBackAsync(int productId, int quantity)
    {
        try
        {
            HttpResponseMessage res = await http.PostAsJsonAsync($"api/stock/batches/{productId}/return", new { quantity });
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<StockBatch?> GetBatchAsync(int productId)
    {
        try
        {
            HttpResponseMessage res = await http.GetAsync($"api/stock/batches/{productId}");
            return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<StockBatch>() : null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
    
    public async Task<List<StockBatch>?> GetBatchesAsync()
    {
        try
        {
            return await http.GetFromJsonAsync<List<StockBatch>>("api/stock/batches");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<TakeStockResult> TakeFromBatchAsync(int productId, int quantity)
    {
        try
        {
            HttpResponseMessage res = await http.PostAsJsonAsync($"api/stock/batches/{productId}/take", new { quantity });
            return res.StatusCode switch
            {
                HttpStatusCode.Accepted => TakeStockResult.Taken,
                HttpStatusCode.Conflict => TakeStockResult.Unavailable,
                HttpStatusCode.NotFound => TakeStockResult.NotFound,
                _ => TakeStockResult.Unavailable
            };
        }
        catch (HttpRequestException)
        {
            return TakeStockResult.NotEnough;
        }        
    }

    public async Task<DeleteStockResult> DeleteFromBatchAsync(int productId)
    {
        try
        {
            HttpResponseMessage res = await http.DeleteAsync($"api/stock/batches/{productId}");
            return res.StatusCode switch
            {
                HttpStatusCode.NoContent => DeleteStockResult.Deleted,
                HttpStatusCode.Conflict => DeleteStockResult.HasActiveReservation,
                HttpStatusCode.NotFound => DeleteStockResult.HasInactiveReservation,
                _ => DeleteStockResult.Unavailable
            };
        }
        catch (HttpRequestException)
        {
            return DeleteStockResult.Unavailable;
        }
    }
}
