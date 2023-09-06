using Microsoft.EntityFrameworkCore;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services;

public class ScheduleService : ICrud<Schedule>
{
    private readonly ScheduleDb _db;
    public ScheduleService(ScheduleDb db)
    {
        _db = db;
    }

    public async void Add(Schedule obj)
    {
        _db.Schedules.Add(obj);
        await _db.SaveChangesAsync();
    }

    public async void Delete(ITuple id)
    {
        _db.Schedules.Remove(await Get(id));
        await _db.SaveChangesAsync();
    }

    public async Task<Schedule> Get(ITuple id) => await EagerLoad().SingleOrDefaultAsync(s => s.Id == (int)id[0]!) 
        ?? throw new InvalidOperationException();

    public async Task<IEnumerable<Schedule>> GetAll() => await EagerLoad().ToListAsync();
    
    private IQueryable<Schedule> EagerLoad()
        => _db.Schedules.Include(s => s.Rooms)
                        .Include(s => s.Reservations);
    public async Task<Schedule> GetLazy(ITuple id)
        => await LazyLoad().SingleOrDefaultAsync(s => s.Id == (int)id[0]!)
        ?? throw new InvalidOperationException($"No {nameof(Schedule)} object can be found with id: {id}.");

    private IQueryable<Schedule> LazyLoad()
        => _db.Schedules;

    public async Task<Schedule> Update(Schedule obj)
    {
        var newSchedule = _db.Schedules.Update(obj);
        await _db.SaveChangesAsync();
        return newSchedule.Entity;
    }
}
