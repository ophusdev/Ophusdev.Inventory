using Inventory.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repository;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasKey(x => x.Id);
        modelBuilder.Entity<Room>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Roma", MaxCapacity = 4, PricePerNight = 180, RoomCategory = 0, IsAvailable = true },
            new Room { Id = 2, Name = "Parigi", MaxCapacity = 4, PricePerNight = 220, RoomCategory = 1, IsAvailable = true },
            new Room { Id = 3, Name = "New York", MaxCapacity = 3, PricePerNight = 440, RoomCategory = 2, IsAvailable = true }
        );
    }

    public DbSet<Room> Rooms { get; set; }
}
