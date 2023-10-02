using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Roster.Data;
using Scheduler.Data.Models;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services;

public class ScheduleService : ICrud<Schedule>
{
    private readonly ScheduleDb _db;
    public ScheduleService(ScheduleDb db)
    {
        _db = db;
    }

    public async Task<bool> Add(Schedule obj)
    {
        _db.Schedules.Add(obj);
        await _db.SaveChangesAsync();
        return true;
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

    public async Task<Schedule> GetNoCycle(ITuple id)
    {
        var schedule = await Get(id);
        foreach (var room in schedule.Rooms!)
        {
            room.RemoveRelations();
        }
        foreach (var reservation in schedule.Reservations!)
        {
            reservation.Schedule = null;
            reservation.Room = null;
        }
        return schedule;
    }

	public async IAsyncEnumerable<Schedule> GetAllNoCycle()
	{
        foreach (var schedule in await GetAll())
        {
            foreach (var room in schedule.Rooms!)
            {
				room.RemoveRelations();
			}
            foreach (var reservation in schedule.Reservations!)
            {
                reservation.Schedule = null;
				reservation.Room = null;
                reservation.People = null;
            }
            yield return schedule;
        }
	}
}
