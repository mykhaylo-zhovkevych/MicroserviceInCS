using MsStore.Users.Api.Domain.Enums;

namespace MsStore.Users.Api.InternalContracts;

public record UserStyleResponse(
    int Id,
    string Name,
    string? Email,
    string? AvatarUrl,
    UsersType Type);
