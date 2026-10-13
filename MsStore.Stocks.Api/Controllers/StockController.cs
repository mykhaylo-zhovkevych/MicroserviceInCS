using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MsStore.Stocks.Api.InternalContracts;
using MsStore.Stocks.Api.Services;
using MsStore.Stocks.Api.Services.Results;

namespace MsStore.Stocks.Api.Controllers;

[ApiController]
[Route ("api/[controller]")]
public class StockController(IInventoryService inventoryService, IReservationService reservationService) : ControllerBase
{
    [HttpPost("batches")]
    public async Task<IActionResult> CreateBatch(CreateBatchRequest req)
    {
        CreatedBatchResult batchResult = await inventoryService.CreateBatchAsync(req.ProductId, req.Category, req.Quantity);
        if (batchResult == CreatedBatchResult.AlreadyExists)
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
        return batches;
    }
    
    [HttpGet("batches/{productId:int}")]
    public async Task<ActionResult<BatchStock>> GetBatch(int productId)
    {
        BatchStock? batch = await inventoryService.GetBatchAsync(productId);
        return batch is null ? NotFound() : batch;
    }
    
    [HttpDelete("batches/{productId:int}")]
    public async Task<IActionResult> DeleteBatch(int productId)
    {
        return await inventoryService.DeleteBatchAsync(productId) switch
        {
            DeletedBatchResult.NotFound => NotFound(),
            DeletedBatchResult.HasActiveReservations => Conflict("Batch has active reservations, release them first"),
            DeletedBatchResult.Deleted => NoContent(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [HttpPost("batches/{productId:int}/take")]
    public async Task<IActionResult> TakeBatch(int productId, [FromQuery, Range(1, 20)] int quantity)
    {
        return await inventoryService.TakeBatchAsync(productId, quantity) switch
        {
            TakeBatchResult.NotFound => NotFound(),
            TakeBatchResult.NotEnough => Conflict("Not enough available stock"),
            TakeBatchResult.Taken => Ok(await inventoryService.GetBatchAsync(productId)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    [HttpPost("batches/{productId:int}/return")]
    public async Task<IActionResult> ReturnBatch(int productId, [FromQuery, Range(1, 20)] int quantity)
    {
        return await inventoryService.ReturnBatchAsync(productId, quantity) switch
        {
            ReturnBatchResult.NotFound => NotFound(),
            ReturnBatchResult.Returned => Ok(await inventoryService.GetBatchAsync(productId)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [HttpPost("reserve")]
    public async Task<ActionResult<ReservationResponse>> ReserveBatch([FromBody] CreateReservationRequest req)
    {
        var (result, reservation) = await reservationService.ReserveBatchAsync(req.UserId, req.Category, req.Quantity);
        return result switch
        {
            CreatedReserveResult.UsersApiUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, "Store.Users.Api is not reachable"),
            CreatedReserveResult.NotFound => BadRequest($"User {req.UserId} does not exist, nothing was reserved"),
            CreatedReserveResult.NotEnough => Conflict($"Not enough available '{req.Category}' across all batches, nothing was reserved"),
            _ => CreatedAtAction(nameof(GetReservation), new { id = reservation!.Id }, reservation)
        };
    }

    [HttpPost("reservations/{id:guid}/release")]
    public async Task<IActionResult> ReservationRelease(Guid id)
    {
        return await reservationService.ReservationReleaseAsync(id) switch
        {
            ReleaseReservationResult.NotFound => NotFound(),
            ReleaseReservationResult.AlreadyReleased => Conflict("Reservation cannot be released"),
            _ => NoContent()
        };
    }

    [HttpGet("reservations")]
    public async Task<ActionResult<List<ReservationResponse>>> GetReservations(int? userId, string? category)
    {
        List<ReservationResponse> reservations = await reservationService.GetReservationsAsync(userId, category);
        return reservations;
    }

    [HttpGet("reservations/{id:guid}")]
    public async Task<ActionResult<ReservationResponse>> GetReservation(Guid id)
    {
        ReservationResponse? reservation = await reservationService.GetReservationAsync(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }
}
