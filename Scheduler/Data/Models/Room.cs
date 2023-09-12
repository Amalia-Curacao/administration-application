using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Number), nameof(ScheduleId))]
public sealed class Room
{
    [Display(Name = "Room number")]
    [Required(ErrorMessage = "Room number is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Room number must be a positive number.")]

    public required int? Number { get; set; }
    [ForeignKey(nameof(Models.Schedule.Id))]
    [Required(ErrorMessage = "Schedule is required.")]
    public int ScheduleId { get; set; }

    public Schedule? Schedule { get; set; }

    [Display(Name = "Room type")]
    [EnumDataType(typeof(RoomType))]
    [Required(ErrorMessage = "Room type is required.")]
    [RegularExpression(@"^(Room|Apartment)$", ErrorMessage = "Please select between type \"Room\" or \"Apartment\".")]
    public required RoomType? Type { get; set; }

    [Display(Name = "Floor number")]
    [Required(ErrorMessage = "Floor number is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Floor number must be a positive number.")]
    public required int? Floor { get; set; }

    [InverseProperty(nameof(Reservation.Room))]
    public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();

    /// <summary>Returns a everyday of a month with the reservation information of a single day.</summary>
    /// <param name="month">Month to return all information.</param>
    /// <returns>Returns the dates and reservations of a month.</returns>
    public IEnumerable<(DateOnly, Reservation[]?)> MonthOverview(DateOnly month)
    {
        for (int day = 1; day <= DateTime.DaysInMonth(year: month.Year, month: month.Month); day++)
        {
            var date = new DateOnly(year: month.Year, month: month.Month, day: day);
            var activeReservations = Reservations!.Where(r => r.Overlap(date)).ToArray();
            activeReservations = activeReservations.Length > 0 ? activeReservations : null;
            yield return (date, activeReservations);
        }
    }
}