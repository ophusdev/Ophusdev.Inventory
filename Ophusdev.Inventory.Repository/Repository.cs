using Inventory.Repository.Abstraction;
using Inventory.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repository;

public class Repository(InventoryDbContext inventoryDbContext) : IRepository
{
    public async Task<int> SaveChangesAsync()
    {
        return await inventoryDbContext.SaveChangesAsync();
    }

    public async Task CreateRoomAsync(string name, decimal pricePerNight, int maxCapacity, int roomCategory, CancellationToken cancellationToken = default)
    {
        Room room = new Room
        {
            Name = name,
            MaxCapacity = maxCapacity,
            RoomCategory = roomCategory,
            PricePerNight = pricePerNight,
            IsAvailable = true,
        };

        await inventoryDbContext.Rooms.AddAsync(room, cancellationToken);
    }

    public async Task<Room?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken = default)
    {
        return await inventoryDbContext.Rooms.Where(r => r.Id == idRoom).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Room>> ReadAllRoomsAsync(CancellationToken cancellationToken = default)
    {
        return await inventoryDbContext.Rooms.ToListAsync(cancellationToken);
    }
}
