using MsStore.Users.Api.Domain.Enums;

namespace MsStore.Users.Api.Domain;

public class RandomUser
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? AvatarUrl { get; set; }
    public UsersType RType { get; set; }
}
