using MsStore.Users.Api.Enum;

namespace MsStore.Users.Api.InternalContracts;

public record UserStyleResponse(
    int Id,
    string Name,
    string? Email,
    string? AvatarUrl,
    UsersType Type);
