using Creative.Api.Data;
using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Api.Data.Models;

[PrimaryKey(nameof(Id))]
[Display(Name = "Guest")]
public sealed class Guest : IModel
{
    public int? Id { get; set; }

    [ForeignKey(nameof(Models.Reservation.Id))]
    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    [Display(Name = "First name")]
    public required string? FirstName { get; set; }

    [Display(Name = "Last name")]
    public required string? LastName { get; set; }
    public int? Age { get; set; }
    public string? Note { get; set; } = "";
    public PersonPrefix? Prefix { get; set; } = PersonPrefix.Unknown;

    /// <inheritdoc/>
    public HashSet<Key> GetPrimaryKey() => new() { new Key(nameof(Id), Id!)};

    public void AutoIncrementPrimaryKey()
        => Id = null;

    /// <inheritdoc/>
    public void SetPrimaryKey(HashSet<Key> keys)
        => Id = keys.Single(key => key.Name == nameof(Id)).Value as int?;

    [Obsolete("Was part of an old implementation for eager loading.")]
	public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class
        => values
            .Include(nameof(Reservation))
            .Include($"{nameof(Reservation)}.{nameof(Models.Reservation.Room)}")
            .Include($"{nameof(Reservation)}.{nameof(Models.Reservation.Schedule)}");
}