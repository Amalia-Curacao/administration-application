using Creative.Api.Data;
using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Api.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class Reservation : IModel
{
    public int? Id { get; set; }

    [Display(Name = "Guest(s)")]
    [InverseProperty(nameof(Guest.Reservation))]
    public ICollection<Guest>? Guests { get; set; } = new List<Guest>();

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
    public required DateOnly? CheckIn { get; set; }

    [Display(Name = "Check out")]
    public required DateOnly? CheckOut { get; set; }

    [Display(Name = "Room type")]
    [EnumDataType(typeof(RoomType))]
    public RoomType? RoomType { get; set; }

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

    [Display(Name = "Total nights")]
    public int? TotalNights() 
        => CheckIn is null || CheckOut is null ? null : CheckOut!.Value.DaysDifference(CheckIn!.Value);

    [Display(Name = "Guests amount")]
    public int GuestsAmount() 
        => Guests?.Count ?? 0;

    /// <summary> Checks if the reservation's check-in and check out overlap with the given date as if the date was a check-in. </summary>
    /// <returns> Returns true if they overlap. </returns>
    /// <exception cref="NullReferenceException"> Thrown when check-in or/and check out is null. </exception>
    public bool OverlapCheckIn(DateOnly date)
    {
        if (CheckIn is null || CheckOut is null)
            throw new NullReferenceException($"Check-in or/and check out is null.");

        return date >= CheckIn && date < CheckOut;
    }

    /// <summary> Checks if the reservation's check-in and check out overlap with the given date as if the date was a check-out. </summary>
    /// <returns> Returns true if they overlap. </returns>
    /// <exception cref="NullReferenceException"> Thrown when check-in or/and check out is null. </exception>"
    public bool OverlapCheckOut(DateOnly date)
    {
		if (CheckIn is null || CheckOut is null)
			throw new NullReferenceException($"Check-in or/and check out is null.");

		return date > CheckIn && date <= CheckOut;
	}

    /// <summary> Checks if the reservation's check-in and check out overlap with the given reservation. </summary>
    /// <returns> Returns true if they overlap. </returns>
    public bool Overlap(Reservation reservation)
        => OverlapCheckIn(reservation.CheckIn!.Value) || OverlapCheckOut(reservation.CheckOut!.Value);

	/// <summary> Removes the relations of the reservation. </summary>
	/// <remarks> This is used to prevent circular reference when serializing to JSON. </remarks>
	[Obsolete("Was part of an old implementation for lazy loading.")]
	public Reservation RemoveRelations()
    {
		Room = null;
		Schedule = null;
		foreach (var guest in Guests!)
        {
			guest.Reservation = null;
		}
		return this;
	}

	/// <inheritdoc/>
	public void AutoIncrementPrimaryKey()
		=> Id = null;

	/// <inheritdoc/>
	[Obsolete("Was part of an old implementation for eager loading.")]
	public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class 
        => values
            .Include(nameof(Room))
            .Include(nameof(Schedule))
            .Include(nameof(Guests));

    /// <inheritdoc/>
	public HashSet<Key> GetPrimaryKey()
        => new() { new Key(nameof(Id), Id) };

    /// <inheritdoc/>
	public void SetPrimaryKey(HashSet<Key> keys)
        => Id = keys.Single(key => key.Name.Equals(nameof(Id))).Value as int?;
}