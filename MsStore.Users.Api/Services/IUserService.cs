using MsStore.Users.Api.Domain;

namespace MsStore.Users.Api.Services;

public interface IUserService
{
    Task<List<RandomUser>> GetUsersAsync();
    Task<RandomUser?> GetUserAsync(int id);
    Task<int> ImportAsync(int count);
}
