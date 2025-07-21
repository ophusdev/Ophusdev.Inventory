using Ophusdev.Inventory.Shared;

namespace Ophusdev.Inventory.Business.Abstraction
{
    public interface IInventoryRoomService
    {
        Task ProcessInventoryRequestAsync(InventoryRequest request);
        Task CompensateInventoryAsync(string bookingId);
    }
}
