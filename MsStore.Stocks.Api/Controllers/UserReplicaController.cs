using Microsoft.AspNetCore.Mvc;
using MsStore.Stocks.Api.Clients;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.Services;

namespace MsStore.Stocks.Api.Controllers;

[ApiController]
[Route("api/stock/users")]
public class UserReplicaController(IUserSyncService userSyncService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserReplica>>> GetUsers()
    {
        return await userSyncService.GetUsersAsync();
    }

    [HttpPost("/api/stock/sync")]
    public async Task<IActionResult> Sync()
    {
        int? synced = await userSyncService.SyncAllAsync();
        return synced is null ? StatusCode(StatusCodes.Status503ServiceUnavailable, "Users.Api is not reachable") : Ok(new { synced });
    }
}
