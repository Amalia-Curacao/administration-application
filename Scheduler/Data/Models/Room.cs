using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Models;

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
    public bool CanFit(Reservation reservation)
        => Reservations!.All(r => !r.Overlap(reservation));

    /// <summary> Adds reservations to the room with out overlap. </summary>
    /// <param name="forceAdd"> If true, the reservation will be added even if it overlaps with another reservation. </param>
    /// <returns> Returns true if reservations has been successfully added to room. </returns>
    public bool AddReservation(Reservation reservation, bool forceAdd = false)
    {
        if (CanFit(reservation) && !forceAdd)
            return false;

        Reservations!.Add(reservation);
        return true;
    }

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

    public ITuple GetPrimaryKey() => Tuple.Create(Number, ScheduleId);

    public void SetPrimaryKey(ITuple key)
    {
        Number = key.Get<int>("Number");
        ScheduleId = key.Get<int>("ScheduleId");
    }

    public void AutoIncrementPrimaryKey() { }

    public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class => values.Include(nameof(Schedule)).Include(nameof(Reservations));
}