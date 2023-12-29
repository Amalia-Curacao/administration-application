using Creative.Api.Data;
using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Api.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class Schedule : IModel
{
    public int? Id { get; set; }

    [InverseProperty(nameof(Reservation.Schedule))]
    public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();

    [InverseProperty(nameof(Room.Schedule))]
    public ICollection<Room>? Rooms { get; set; } = new HashSet<Room>();

    [Display(Name = "Name")]
    public string? Name { get; set; }

	[Obsolete("Was part of an old implementation for eager loading.")]
	public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class 
        => values
        .Include(nameof(Reservations))
        .Include($"{nameof(Reservations)}.{nameof(Reservation.Guests)}")
        .Include(nameof(Rooms));

    public void AutoIncrementPrimaryKey() 
        => Id = null;

	public void SetPrimaryKey(HashSet<Key> keys)
	{
        Id = keys.Single(key => key.Name == nameof(Id)).Value as int?;
	}

	public HashSet<Key> GetPrimaryKey()
	{
		return new HashSet<Key> { new Key(nameof(Id), Id) };
	}
}