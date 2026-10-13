using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Domain;

namespace MsStore.Stocks.Api.Data;

// All mapping lives as data annotations on the Domain classes, so no OnModelCreating needed
public class StockDbContext(DbContextOptions<StockDbContext> options) : DbContext(options)
{
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<UserReservation> Reservations => Set<UserReservation>();
    public DbSet<ReservationLine> ReservationLines => Set<ReservationLine>();
    public DbSet<UserReplica> Users => Set<UserReplica>();
}
