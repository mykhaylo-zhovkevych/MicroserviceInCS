using MsStore.Stocks.Api.Clients;
using MsStore.Stocks.Api.Domain;
using Riok.Mapperly.Abstractions;

namespace MsStore.Stocks.Api.Mapping;

[Mapper]
public static partial class UserMapper
{
    public static partial UserReplica ToReplica(this UserResponse user);

    // Updates an already tracked entity in place, the key is left alone so EF does not complain
    [MapperIgnoreSource(nameof(UserResponse.Id))]
    [MapperIgnoreTarget(nameof(UserReplica.Id))]
    public static partial void UpdateReplica(this UserResponse user, UserReplica target);
}
