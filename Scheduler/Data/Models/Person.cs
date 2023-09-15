using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Id))]
[Display(Name = "Guest")]
public sealed class Person
{
    public int Id { get; set; }

    [ForeignKey(nameof(Models.Reservation.Id))]
    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    [Display(Name = "First name")]

    public required string FirstName { get; set; }

    [Display(Name = "Last name")]
    public required string LastName { get; set; }
    public int Age { get; set; }
    public string? Note { get; set; } = "";
    public PersonPrefix? Prefix { get; set; } = PersonPrefix.Unknown;
}