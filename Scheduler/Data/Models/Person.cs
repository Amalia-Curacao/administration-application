using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Models;

[PrimaryKey(nameof(Id))]
[Display(Name = "Guest")]
public sealed class Person : IModel
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
    public IDictionary<string, object> GetPrimaryKey() => new Dictionary<string, object>() { { nameof(Id), Id! } };

    public void AutoIncrementPrimaryKey()
    {
        Id = null;
    }

    /// <inheritdoc/>
    public void SetPrimaryKey(IDictionary<string, object> primaryKey)
    {
        Id = primaryKey[nameof(Id)] as int?;
    }

    public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class
        => values
        .Include(nameof(Reservation))
        .Include($"{nameof(Reservation)}.{nameof(Reservation.Room)}")
        .Include($"{nameof(Reservation)}.{nameof(Reservation.Schedule)}");
}