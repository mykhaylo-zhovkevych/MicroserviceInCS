using Microsoft.EntityFrameworkCore;
using MsStore.Stocks.Api.Domain;

namespace MsStore.Stocks.Api.Data;

// All mapping lives as data annotations on the Domain classes, so no OnModelCreating needed
public class StockDb(DbContextOptions<StockDb> options) : DbContext(options)
{
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationLine> ReservationLines => Set<ReservationLine>();
}
