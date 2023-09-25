using Creative.Database;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data.Models;

namespace Roster.Data;

public class ScheduleDb : SqlServerContext
{
    public ScheduleDb(string connectionString) : base(connectionString)
    {
    }

    public ScheduleDb(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Person> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => new { r.RoomNumber, r.ScheduleId })
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasMany(r => r.People)
            .WithOne(p => p.Reservation)
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
