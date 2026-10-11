using Microsoft.EntityFrameworkCore;
using MsStore.Users.Api.Data;
using MsStore.Users.Api.Domain;

namespace MsStore.Users.Api.Services;

public class UserService(UsersDbContext db) : IUserService
{
    public async Task<List<RandomUser>> GetUsersAsync()
    {
        return await db.Users.AsNoTracking().ToListAsync();
    }

    public async Task<RandomUser?> GetUserAsync(int id)
    {
        return await db.Users.FindAsync(id);
    }
    
    public async Task<int> ImportAsync(int count)
    {
        List<RandomUser> users = UserContextGenerator.Generate(count);
        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        return users.Count;
    }
}
