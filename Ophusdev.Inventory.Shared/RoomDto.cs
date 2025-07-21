namespace Inventory.Shared
{
    public enum RoomCategoryEnum: ushort
    {
        Standard = 0,
        Deluxe = 1,
        Suite = 2,
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public decimal PricePerNight { get; set; }

        public int MaxCapacity { get; set; }

        public int RoomCategory { get; set; }
    }
}
