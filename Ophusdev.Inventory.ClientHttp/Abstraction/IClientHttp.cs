using Inventory.Shared;

namespace Inventory.ClientHttp.Abstraction;

public interface IClientHttp
{
    Task<string?> CreateRoomAsync(RoomDto room, CancellationToken cancellationToken = default);
    Task<RoomDto?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken = default);
    Task<List<RoomDto>?> GetAllRoomAsync(CancellationToken cancellationToken = default);
}
