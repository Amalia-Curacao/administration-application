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

    public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class 
        => values
        .Include(nameof(Reservations))
        .Include($"{nameof(Reservations)}.{nameof(Reservation.People)}")
        .Include(nameof(Rooms));

    public void AutoIncrementPrimaryKey() 
        => Id = null;

    public IDictionary<string, object> GetPrimaryKey() 
        => new Dictionary<string, object> { { nameof(Id), Id! } };

    public void SetPrimaryKey(IDictionary<string, object> primaryKey) 
        => Id = primaryKey[nameof(Id)] as int?;
}