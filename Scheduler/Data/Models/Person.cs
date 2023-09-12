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
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "A person's first name is required to be minimum of 1 letter and a maximum of 50 letter.")]

    public required string FirstName { get; set; }

    [Display(Name = "Last name")]
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "A person's last name is required to be minimum of 1 letter and a maximum of 50 letter.")]
    public required string LastName { get; set; }

    [Range(0, 120, ErrorMessage = $"A {nameof(Person)} cannot be older than 120 or younger than 0 years old.")]
    public int Age { get; set; }
    public string? Note { get; set; } = "";
    public PersonPrefix? Prefix { get; set; } = PersonPrefix.Unknown;
}