using Inventory.Repository.Model;
using System.ComponentModel.DataAnnotations;

namespace Ophusdev.Inventory.Repository.Model;

public enum ReservationStatus
{
    [Display(Name = "Free")]
    Free,
    [Display(Name = "Reserved")]
    Reserved,
}

public class Reservation
{
    public int Id { get; set; }
    public required string SagaId { get; set; }
    public required string BookingId { get; set; }
    public int RoomId { get; set; }   // FK Property
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestId { get; set; }
    public ReservationStatus Status { get; set; }

    public Room? Room { get; set; } // Navigation Property
}

