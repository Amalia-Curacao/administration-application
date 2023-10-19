using Creative.Database;
using Creative.Database.Data;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data.Models;

namespace Roster.Data;

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
        options.DbOptions = SqliteContextTool.InitDbContextOptions<ScheduleDb>(DbPath(options.DbName));
        return new ScheduleDb(options);
    }

    private static ScheduleDb InitializeAsSqlServer(SqlServerOptions options)
    {
        options.DbOptions = SqlServerContextTool.InitDbContextOptions<ScheduleDb>(options.ConnectionString);
        return new ScheduleDb(options);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        switch (Options.DatabaseSrc)
        {
            case DatabaseSrc.Sqlite:
                SqliteContextTool.OnConfiguring(optionsBuilder, DbPath());
                break;
            case DatabaseSrc.SqlServer:
                SqlServerContextTool.OnConfiguring(optionsBuilder, ((SqlServerOptions)Options).ConnectionString);
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

        modelBuilder.Entity<Reservation>().Property(e => e.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Person>().Property(e => e.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Schedule>().Property(e => e.Id).ValueGeneratedOnAdd();
    }
}
