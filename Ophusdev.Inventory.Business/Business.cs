using Inventory.Business.Abstraction;
using Inventory.Repository.Abstraction;
using Inventory.Repository.Model;
using Inventory.Shared;
using Microsoft.Extensions.Logging;

namespace Inventory.Business;

public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    public ILogger<Business> Logger { get; } = logger;

    public async Task CreateRoomAsync(RoomDto room, CancellationToken cancellationToken)
    {
        await repository.CreateRoomAsync(room.Name, room.PricePerNight, room.MaxCapacity, ((int)room.RoomCategory));

        await repository.SaveChangesAsync();
    }

    public async Task<RoomDto?> ReadRoomAsync(int idRoom, CancellationToken cancellationToken)
    {
        var room = await repository.ReadRoomAsync(idRoom);

        if (room is null)
            return null;

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            PricePerNight = room.PricePerNight,
            MaxCapacity = room.MaxCapacity,
            RoomCategory = room.RoomCategory,
            IsAvailable = room.IsAvailable
        };
    }

    public async Task<List<RoomDto>> ReadAllRoomsAsync(CancellationToken cancellationToken)
    {
        var rooms_db = await repository.ReadAllRoomsAsync();

        List<RoomDto> rooms = new List<RoomDto>();

        foreach (Room room in rooms_db)
        {
            rooms.Add(new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                PricePerNight = room.PricePerNight,
                MaxCapacity = room.MaxCapacity,
                RoomCategory = room.RoomCategory,
                IsAvailable = room.IsAvailable
            });
        }
        return rooms;
    }
}
