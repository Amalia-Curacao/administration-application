using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roster.Models;

public sealed class Reservation
{
    [Key]
    [Required]
    public int Id { get; set; }

    [ForeignKey(nameof(Person.Id))]
    public int[]? PeopleId { get; set; }
    [InverseProperty(nameof(Person))]
    public ICollection<Person>? People { get; set; }

    [Required]
    public required DateOnly CheckIn { get; set; }
    [Required]
    public required DateOnly CheckOut { get; set; }
    [Required]
    public required RoomType RoomType { get; set; }
    [Required]
    [ForeignKey(nameof(Models.Room))]
    public required int RoomId { get; set; }
    public Room Room { get; set; }

}