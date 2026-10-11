using Microsoft.AspNetCore.Mvc;
using MsStore.Users.Api.Domain;
using MsStore.Users.Api.InternalContracts;
using MsStore.Users.Api.Services;

namespace MsStore.Users.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserStyleResponse>>> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return users.Select(ToStyleResponse).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserStyleResponse>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return ToStyleResponse(user);
    }
    
    [HttpPost("import")]
    public async Task<IActionResult> CreateGeneration(int count)
    {
        if (count is < 1 or > 100)
        {
            return BadRequest("count must be between 1 and 100");
        }

        int imported = await userService.ImportAsync(count);
        return Ok(new { imported });
    }

    private static UserStyleResponse ToStyleResponse(RandomUser user) => new(user.Id, user.Name, user.Email, user.AvatarUrl, user.RType);
}
