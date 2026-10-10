using Bogus;
using MsStore.Users.Api.Domain;
using MsStore.Users.Api.Enum;

namespace MsStore.Users.Api.Services;

public static class UserContextGenerator
{
    public static List<RandomUser> Generate(int count)
    {
        Randomizer.Seed = new Random(8675309);

        var faker = new Faker<RandomUser>()
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) =>
            {
                string[] parts = u.Name.Split(' ');
                return f.Internet.Email(parts[0], parts[^1]);
            })
            .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
            .RuleFor(u => u.RType, f => f.Random.WeightedRandom(
                new[] { UsersType.User, UsersType.Admin },
                new[] { 0.9f, 0.1f }));                       

        return faker.Generate(count);
    }
}