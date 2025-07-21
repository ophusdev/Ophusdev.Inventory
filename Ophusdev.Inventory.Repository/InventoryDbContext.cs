using Inventory.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Ophusdev.Inventory.Repository.Model;

namespace Inventory.Repository;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasKey(x => x.Id);
        modelBuilder.Entity<Room>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Reservation>().HasKey(x => x.Id);
        modelBuilder.Entity<Reservation>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Roma", MaxCapacity = 4, PricePerNight = 180, RoomCategory = 0},
            new Room { Id = 2, Name = "Parigi", MaxCapacity = 4, PricePerNight = 220, RoomCategory = 1 },
            new Room { Id = 3, Name = "New York", MaxCapacity = 3, PricePerNight = 440, RoomCategory = 2}
        );

        modelBuilder.Entity<Reservation>()
        .HasOne(r => r.Room)
        .WithMany(room => room.Reservations)
        .HasForeignKey(r => r.RoomId);
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}
