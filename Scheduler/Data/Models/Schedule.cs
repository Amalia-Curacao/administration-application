using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class Schedule
{
    public int Id { get; set; }

    [InverseProperty(nameof(Reservation.Schedule))]
    public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();

    [InverseProperty(nameof(Room.Schedule))]
    public ICollection<Room>? Rooms { get; set; } = new HashSet<Room>();

}