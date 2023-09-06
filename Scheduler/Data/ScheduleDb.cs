using Creative.Database;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data.Models;

namespace Roster.Data;

public class ScheduleDb : SqliteContext
{
    public ScheduleDb() : base("Schedule")
    {
    }

    public ScheduleDb(DbContextOptions options) : base("Schedule", options)
    {
    }

    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
}
