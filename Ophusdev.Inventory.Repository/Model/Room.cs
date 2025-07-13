using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Repository.Model;

public class Room
{
    public int Id { get; set; }
    public required string Name { get; set; }
    [Precision(5, 2)]
    public decimal PricePerNight { get; set; }
    public int MaxCapacity { get; set; }
    // @see Inventory.Shared.RoomDto.RoomCategoryEnum
    public int RoomCategory { get; set; }
    public bool IsAvailable { get; set; }

}
