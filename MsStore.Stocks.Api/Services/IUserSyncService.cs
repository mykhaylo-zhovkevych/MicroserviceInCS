using MsStore.Stocks.Api.Domain;

namespace MsStore.Stocks.Api.Services;

public interface IUserSyncService
{
    Task<List<UserReplica>> GetUsersAsync();
    Task<int?> SyncAllAsync();
}
