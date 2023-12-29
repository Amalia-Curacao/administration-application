using Creative.Api.Data;
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
        => Reservations!.All(r => !r.Overlap(reservation) || reservation.GetPrimaryKey().SetEquals(r.GetPrimaryKey()));

    /// <inheritdoc cref="ICollection{T}.Remove(T)"/>
    public bool RemoveReservation(Reservation reservation)
        => Reservations!.Remove(reservation);

    /// <inheritdoc cref="ICollection{T}.Add(T)"/>
    public bool AddReservation(Reservation reservation)
    {
		if (CanFit(reservation))
        {
			Reservations!.Add(reservation);
			return true;
		}
		return false;
	}

	/// <summary> Removes relations with objects. </summary>
	/// <remarks> This method is used to avoid circular references when serializing to JSON. </remarks>
	[Obsolete("Was part of an old implementation for lazy loading.")]
	public Room RemoveRelations()
    {
        Reservations = null; 
        Schedule = null;
        return this;
    }

    public void AutoIncrementPrimaryKey() { }

	[Obsolete("Was part of an old implementation for eager loading.")]
	public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class 
        => values
        .Include(nameof(Schedule))
        .Include(nameof(Reservations))
        .Include($"{nameof(Reservations)}.{nameof(Reservation.Guests)}");

	public HashSet<Key> GetPrimaryKey()
	{
        return new HashSet<Key>() { new Key(nameof(Number), Number), new Key(nameof(ScheduleId), ScheduleId) };
	}

	public void SetPrimaryKey(HashSet<Key> keys)
	{
        Number = (int)keys.Single(key => key.Name.Equals(nameof(Number))).Value!;
		ScheduleId = (int)keys.Single(key => key.Name.Equals(nameof(ScheduleId))).Value!;
	}
}