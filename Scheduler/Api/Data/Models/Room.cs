using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Api.Data.Models;

[PrimaryKey(nameof(Number), nameof(ScheduleId))]
public sealed class Room : IModel
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
    public ICollection<Reservation>? Reservations { get; private set; } = new List<Reservation>();

    /// <summary> Checks if the room can fit the reservation. </summary>
    /// <returns> Returns true if the room can fit the reservation. </returns>
    /// <remarks> Will not detect reservations with the same primary key as overlapping. </remarks>
    public bool CanFit(Reservation reservation)
        => Reservations!.All(r => !r.Overlap(reservation) || reservation.GetPrimaryKey().ContentEquals(r.GetPrimaryKey()));

    /// <inheritdoc cref="ICollection{T}.Remove(T)"/>
    public bool RemoveReservation(Reservation reservation)
        => Reservations!.Remove(reservation);

    /// <summary> Removes relations with objects. </summary>
    /// <remarks> This method is used to avoid circular references when serializing to JSON. </remarks>
    public Room RemoveRelations()
    {
        Reservations = null; 
        Schedule = null;
        return this;
    }

    public IDictionary<string, object> GetPrimaryKey() 
        => new Dictionary<string, object> { { nameof(Number), Number! }, { nameof(ScheduleId), ScheduleId } };

    public void SetPrimaryKey(IDictionary<string, object> key)
    {
        Number = (int)key[nameof(Number)];
        ScheduleId = (int)key[nameof(ScheduleId)];
    }

    public void AutoIncrementPrimaryKey() { }

    public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class 
        => values
        .Include(nameof(Schedule))
        .Include(nameof(Reservations))
        .Include($"{nameof(Reservations)}.{nameof(Reservation.People)}");
}