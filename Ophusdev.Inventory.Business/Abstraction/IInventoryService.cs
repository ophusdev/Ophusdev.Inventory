using Ophusdev.Inventory.Shared;

namespace Ophusdev.Inventory.Business.Abstraction
{
    public interface IInventoryService
    {
        Task ProcessInventoryRequestAsync(InventoryRequest request);
        Task CompensateInventoryAsync(CompensationRequest request);
    }
}
