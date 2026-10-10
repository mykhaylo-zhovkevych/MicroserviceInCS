using Microsoft.EntityFrameworkCore;
using MsStore.Users.Api.Domain;

namespace MsStore.Users.Api.Data;

// probably issues with naming 
public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<RandomUser> Users => Set<RandomUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RandomUser>(u =>
        {
            u.HasIndex(x => x.Id).IsUnique();
            u.Property(x => x.Type).HasConversion<string>().HasMaxLength(16);
        });
    }
}
