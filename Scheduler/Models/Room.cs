using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roster.Models;

public sealed class Room
{
    [Key]
    public required int Id { get; set; }
    [Required]
    public required RoomType Type { get; set; }
    [Required]
    public required int Number { get; set; }
    [Required]
    public required int Floor { get; set; }
    [ForeignKey(nameof(Reservation.Id))]
    public int[] ReservationIds { get; set; }
    [InverseProperty(nameof(Reservation))]
    public ICollection<Reservation>? Reservations { get; set; }
}