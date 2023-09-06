using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class Reservation
{
    public int Id { get; set; }

    [Display(Name = "People")]
    [InverseProperty(nameof(Person.Reservation))]
    public ICollection<Person>? People { get; set; } = new List<Person>();

    [Required(ErrorMessage = "Room is required.")]
    [Display(Name = "Room")]
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

    public bool Overlap(DateOnly date)
    {
        if (CheckIn is null || CheckOut is null)
            return false;

        return date >= CheckIn && date <= CheckOut;
    }

}