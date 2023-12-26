using Creative.Database;
using Creative.Database.Data;
using Microsoft.EntityFrameworkCore;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Data;

public class ScheduleDb : DatabaseContext
{
    private ScheduleDb(DatabaseContextOptions options) : base(options)
    {

    }
    public ScheduleDb(DbContextOptions options) : base(options)
    {

    }

    public static ScheduleDb Create(DatabaseContextOptions options)
    => options.DatabaseSrc switch
    {
        DatabaseSrc.Sqlite => InitializeAsSqlite((SqliteOptions)options),
        DatabaseSrc.SqlServer => InitializeAsSqlServer((SqlServerOptions)options),
        _ => throw new NotImplementedException("Database src is not supported.")
    };
    
    private static ScheduleDb InitializeAsSqlite(SqliteOptions options)
    {
        options.DbOptions = SqliteContextTool.InitDbContextOptions<ScheduleDb>(options);
        return new ScheduleDb(options);
    }

    private static ScheduleDb InitializeAsSqlServer(SqlServerOptions options)
    {
        options.DbOptions = SqlServerContextTool.InitDbContextOptions<ScheduleDb>(options);
        return new ScheduleDb(options);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        switch (Options.DatabaseSrc)
        {
            case DatabaseSrc.Sqlite:
                SqliteContextTool.OnConfiguring(optionsBuilder, (SqliteOptions)Options);
                break;
            case DatabaseSrc.SqlServer:
                SqlServerContextTool.OnConfiguring(optionsBuilder, (SqlServerOptions)Options);
                break;
            default:
                throw new NotImplementedException("Database src is not supported.");
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        switch (Options.DatabaseSrc)
        {
            case DatabaseSrc.SqlServer:
                SqlServerContextTool.ConfigureConventions(configurationBuilder);
                break;
            default:
                break;
        }
    }

    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Guest> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => new { r.RoomNumber, r.ScheduleId })
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasMany(r => r.Guests)
            .WithOne(p => p.Reservation)
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Schedule)
			.WithMany(s => s.Reservations)
			.HasForeignKey(r => r.ScheduleId)
			.OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.Schedule)
			.WithMany(s => s.Rooms)
			.HasForeignKey(r => r.ScheduleId)
			.OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Reservations)
			.WithOne(r => r.Room)
			.HasForeignKey(r => new { r.RoomNumber, r.ScheduleId })
			.OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Schedule>()
			.HasMany(s => s.Reservations)
			.WithOne(r => r.Schedule)
			.HasForeignKey(r => r.ScheduleId)
			.OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Schedule>()
            .HasMany(s => s.Rooms)
            .WithOne(r => r.Schedule)
            .HasForeignKey(r => r.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Guest>()
            .HasOne(p => p.Reservation)
			.WithMany(r => r.Guests)
			.HasForeignKey(p => p.ReservationId)
			.OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>().Property(e => e.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Guest>().Property(e => e.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Schedule>().Property(e => e.Id).ValueGeneratedOnAdd();
    }
}
