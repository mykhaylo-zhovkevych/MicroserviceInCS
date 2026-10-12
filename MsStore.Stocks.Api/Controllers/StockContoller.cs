using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    [HttpDelete("batches/{productId:int}")]
    public async Task<ActionResult<BatchStock>> DeleteBatch(int productId)
    {
        return await inventoryService.DeleteBatchAsync(productId) switch
        {
            DeletedResult.NotFound => NotFound(),
            DeletedResult.HasActiveReservations => Conflict("Batch has active reservations, release them first"),
            _ => NotFound()
        };
    }

    [HttpPost("batches/{productId:int}/take")]
    public async Task<IActionResult> TakeBatch(int productId, [FromQuery, Range(1, 20)] int quantity)
    {
        return await inventoryService.TakeBatchAsync(productId, quantity) switch
        {
            TakeResult.BatchNotFound => NotFound(),
            TakeResult.NotEnough => Conflict("Not enough available stock"),
            _ => Ok(await inventoryService.GetBatchAsync(productId))
        };
    }
    
    [HttpPost("batches/{productId:int}/give-back")]
    public async Task<IActionResult> GiveBack(int productId, [FromQuery, Range(1, 20)] int quantity)
    {
        return await inventoryService.GiveBackAsync(productId, quantity)
            ? Ok(await inventoryService.GetBatchAsync(productId))
            : NotFound();
    }

    [HttpPost("reserve")]
    public async Task<ActionResult<ReservationResponse>> ReserveBatch([FromBody] CreateReservationRequest req)
    {
        var (result, reservation) = await inventoryService.ReserveBatchAsync(req.Category, req.Quantity, req.UserId);
        return result switch
        {
            CreatedReserveResult.UsersApiUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable, "Store.Users.Api is not reachable"),
            CreatedReserveResult.UserNotFound => BadRequest($"User {req.UserId} does not exist, nothing was reserved"),
            CreatedReserveResult.NotEnough => Conflict($"Not enough available '{req.Category}' across all batches, nothing was reserved"),
            _ => CreatedAtAction(nameof(GetReservation), new { id = reservation!.Id }, reservation)
        };
    }

    [HttpPost("reservations/{id:guid}/release")]
    public async Task<IActionResult> ReservationRelease(Guid id)
    {
        return await inventoryService.ReservationReleaseAsync(id) switch
        {
            ReleaseResult.NotFound => NotFound(),
            ReleaseResult.AlreadyReleased => Conflict("Reservation cannot be released"),
            ReleaseResult.NotOwner => Conflict(""),
            _ => NoContent()
        };
    }

    [HttpGet("reservations")]
    public async Task<ActionResult<List<ReservationResponse>>> GetReservations(int? userId, string? category)
    {
        List<ReservationResponse> reservations = await inventoryService.GetReservationsAsync(userId, category);
        return reservations is null ? NoContent() : Ok(reservations);

    }

    [HttpGet("reservations/{id:guid}")]
    public async Task<ActionResult<ReservationResponse>> GetReservation(Guid id)
    {
        ReservationResponse? reservation = await inventoryService.GetReservationAsync(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

}