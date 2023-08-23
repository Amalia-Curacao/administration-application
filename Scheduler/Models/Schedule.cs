using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roster.Models;

public sealed class Schedule
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Reservation.Id))]
    public int[]? ReservationIds { get; set; }
    [InverseProperty(nameof(Reservation))]
    public ICollection<Reservation>? Reservations { get; set; }

    [ForeignKey(nameof(Room.Id))]
    public int[]? RoomIds { get; set; }
    [InverseProperty(nameof(Room))]
    public ICollection<Room>? Rooms { get; set; }

}