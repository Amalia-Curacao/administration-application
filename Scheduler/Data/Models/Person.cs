using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class Person
{
    public int Id { get; set; }

    [ForeignKey(nameof(Models.Reservation.Id))]
    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "A person's name is required to be minimum of 1 letter and a maximum of 50 letter.")]

    public required string Name { get; set; }
    [Range(0, 120, ErrorMessage = $"A {nameof(Person)} cannot be older than 120 or younger than 0 years old.")]
    public int Age { get; set; }
    public string? Note { get; set; } = "";
    public PersonPrefix? Prefix { get; set; } = PersonPrefix.Unknown;
}