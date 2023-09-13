using Microsoft.EntityFrameworkCore;
using Scheduler.Data.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class Reservation
{
    public int Id { get; set; }

    [Display(Name = "Guest(s)")]
    [InverseProperty(nameof(Person.Reservation))]
    public ICollection<Person>? People { get; set; } = new List<Person>();

    [Required(ErrorMessage = "Room is required.")]
    [Display(Name = "Room #")]
    [ForeignKey(nameof(Models.Room.Number))]
    public int? RoomNumber { get; set; }

    public Room? Room { get; set; }

    [Display(Name = "Schedule")]
    [ForeignKey(nameof(Models.Room.ScheduleId))]
    public int? RoomScheduleId { get; set; }

    [Display(Name = "Schedule")]
    [ForeignKey(nameof(Models.Schedule.Id))]
    public int? ScheduleId { get; set; }

    public Schedule? Schedule { get; set; }

    [Display(Name = "Check-in")]
    [Required(ErrorMessage = "Check-in date is required.")]
    public required DateOnly? CheckIn { get; set; }

    [Display(Name = "Check out")]
    [Required(ErrorMessage = "Check out date is required.")]
    public required DateOnly? CheckOut { get; set; }

    [Display(Name = "Room type")]
    [EnumDataType(typeof(RoomType))]
    [Required(ErrorMessage = "Room type is required.")]
    [RegularExpression(@"^(Room|Apartment)$", ErrorMessage = "Please select between type \"Room\" or \"Apartment\".")]
    public required RoomType? RoomType { get; set; }

    [Display(Name = "Total nights")]
    public int? TotalNights() => CheckOut!.Value.DaysDifference(CheckIn!.Value);

    [Display(Name = "Guests amount")]
    public int GuestsAmount() => People?.Count ?? 0;

    [Display(Name = "Flight arrival #")]
    public string? FlightArrivalNumber { get; set; }

    [Display(Name = "Flight departure #")]
    public string? FlightDepartureNumber { get; set; }

    [Display(Name = "Flight arrival time")]
    public TimeOnly? FlightArrivalTime { get; set; }

    [Display(Name = "Flight departure time")]
    public TimeOnly? FlightDepartureTime { get; set; }

    [Display(Name = "Booking source")]
    [EnumDataType(typeof(BookingSource))]
    public BookingSource? BookingSource { get; set; }

    [Display(Name = "Remarks")]
    public string? Remarks { get; set; }
    public bool Overlap(DateOnly date)
    {
        if (CheckIn is null || CheckOut is null)
            return false;

        return date >= CheckIn && date <= CheckOut;
    }

}