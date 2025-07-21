using Inventory.Repository.Abstraction;
using Inventory.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Ophusdev.Inventory.Repository.Model;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;

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

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, CancellationToken cancellationToken = default)
    {
        return !inventoryDbContext.Reservations.Any(r =>
            r.RoomId == roomId &&
            r.CheckInDate <= checkOutDate &&
            r.CheckOutDate >= checkInDate &&
            r.Status == ReservationStatus.Reserved
        );
    }

    public async Task ReserveRoomAsync(string bookingId, string sagaId, int roomId, DateTime checkInDate, DateTime checkOutDate, int guestId, ReservationStatus status, CancellationToken cancellationToken = default)
    {
        Reservation reservation = new Reservation
        {
            RoomId = roomId,
            BookingId = bookingId,
            SagaId = sagaId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            GuestId = guestId,
            Status = status
        };

        await inventoryDbContext.Reservations.AddAsync(reservation, cancellationToken);
    }
    public async Task<Reservation> GetByBookingIdAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await inventoryDbContext.Reservations.Where(b => b.BookingId == bookingId).FirstAsync();
    }

    public async Task ReleaseRoomAsync(string bookingId)
    {
        var reservationItem = await GetByBookingIdAsync(bookingId);

        reservationItem.Status = ReservationStatus.Free;

        await inventoryDbContext.SaveChangesAsync();
    }
}
