using System.Net;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using MsStore.Orders.Api.Client.Results;

namespace MsStore.Orders.Api.Client;

// Represents an intermediate exchange value
public record StockBatch(int ProductId, string Category, int InStock, int Available, int Capacity);


public class StockClient(HttpClient http) : IStockClient
{
    public async Task<bool> CreateBatchAsync(int productId, string category, int quantity)
    {
        try
        {
            HttpResponseMessage res = await http.PostAsJsonAsync("api/stock/batches", new { productId, category, quantity});
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<bool> ReturnBatchAsync(int productId, int quantity)
    {
        try
        {
            HttpResponseMessage res = await http.PostAsync($"api/stock/batches/{productId}/return?quantity={quantity}", null);
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

    public async Task<TakeStockResult> TakeBatchAsync(int productId, int quantity)
    {
        try
        {
            HttpResponseMessage res = await http.PostAsync($"api/stock/batches/{productId}/take?quantity={quantity}", null);
            return res.StatusCode switch
            {
                HttpStatusCode.OK => TakeStockResult.Taken,
                HttpStatusCode.Conflict => TakeStockResult.NotEnough,
                HttpStatusCode.NotFound => TakeStockResult.NotFound,
                _ => TakeStockResult.Unavailable
            };
        }
        catch (HttpRequestException)
        {
            return TakeStockResult.Unavailable;
        }        
    }

    public async Task<DeleteStockResult> DeleteBatchAsync(int productId)
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
