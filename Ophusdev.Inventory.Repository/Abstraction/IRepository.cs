using Inventory.Repository.Model;

namespace Inventory.Repository.Abstraction;

public interface IRepository
{
    Task<int> SaveChangesAsync();
    Task CreateRoomAsync(string name, decimal pricePerNight, int capacity, int roomCategory, CancellationToken cancellationToken = default);
    Task<Room?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken = default);
    Task<List<Room>> ReadAllRoomsAsync(CancellationToken cancellationToken = default);
}
