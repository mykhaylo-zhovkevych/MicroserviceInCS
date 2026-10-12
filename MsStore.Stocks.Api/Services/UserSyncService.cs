using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Clients;
using MsStore.Stocks.Api.Data;
using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.Mapping;

namespace MsStore.Stocks.Api.Services;

// Keeps the local UserReplica table in line with Users.Api, so reservations can check users without a network call
public class UserSyncService(StockDb db, UserClient userClient)
{
    public async Task<List<UserReplica>> GetUsersAsync()
    {
        return await db.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .ToListAsync();
    }

    public async Task<int?> SyncAllAsync()
    {
        List<UserResponse>? remoteUsers = await userClient.GetUsersAsync();
        if (remoteUsers is null)
        {
            return null;
        }

        // One query for all replicas instead of a FindAsync per user
        Dictionary<int, UserReplica> replicas = await db.Users.ToDictionaryAsync(u => u.Id);

        foreach (UserResponse remote in remoteUsers)
        {
            if (replicas.TryGetValue(remote.Id, out UserReplica? replica))
            {
                remote.UpdateReplica(replica);
            }
            else
            {
                db.Users.Add(remote.ToReplica());
            }
        }

        await db.SaveChangesAsync();
        return remoteUsers.Count;
    }
}
