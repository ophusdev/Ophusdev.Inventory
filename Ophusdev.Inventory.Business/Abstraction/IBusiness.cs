using Inventory.Shared;

namespace Inventory.Business.Abstraction;

public interface IBusiness
{
    Task CreateRoomAsync(RoomDto room, CancellationToken cancellationToken = default);
    Task<RoomDto?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken = default);
    Task<List<RoomDto>> ReadAllRoomsAsync(CancellationToken cancellationToken = default);
}
