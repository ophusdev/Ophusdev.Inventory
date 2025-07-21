using Inventory.Repository.Model;
using Ophusdev.Inventory.Repository.Model;

namespace Inventory.Repository.Abstraction;

public interface IRepository
{
    Task<int> SaveChangesAsync();
    Task CreateRoomAsync(string name, decimal pricePerNight, int capacity, int roomCategory, CancellationToken cancellationToken = default);
    Task<Room?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken = default);
    Task<List<Room>> ReadAllRoomsAsync(CancellationToken cancellationToken = default);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, CancellationToken cancellation = default);
    Task ReserveRoomAsync(string bookingId, string sagaId, int roomId, DateTime checkInDate, DateTime checkOutDate, int guestId, ReservationStatus status, CancellationToken cancellationToken = default);
    Task ReleaseRoomAsync(string bookingId);
}
