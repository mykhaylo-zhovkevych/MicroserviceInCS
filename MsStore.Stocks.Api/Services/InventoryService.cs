using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using MsStore.Stocks.Api.Clients;
using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.Domain.Enums;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Services;

public class InventoryService(StockDbContext db))
{

    public Task<CreatedBatchResult> CreateBatchAsync(int productId, string category, int quantity)
    {
        throw new NotImplementedException();
    }
    
    public Task<List<BatchStock>> GetBatchesAsync()
    {
        throw new NotImplementedException();
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

    public Task ReturnBatchAsync()
    {
        throw new NotImplementedException();
    }
    
    public Task<bool> GiveBackAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }
    
    public Task<(CreatedReserveResult Result, ReservationResponse? Reservation)> ReserveBatchAsync(string category, int quantity, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<ReleaseReservationResult> ReservationReleaseAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ReservationResponse>> GetReservationsAsync(int? userId, string? category)
    {
        throw new NotImplementedException();
    }

    public Task<ReservationResponse?> GetReservationAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
