using Microsoft.EntityFrameworkCore;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services;

public class RoomService : ICrud<Room>
{
    private readonly ScheduleDb _db;
    public RoomService(ScheduleDb db)
    {
        _db = db;
    }
    public async void Add(Room obj)
    {
        _db.Rooms.Add(obj);
        await _db.SaveChangesAsync();
    }

    public async void Delete(ITuple id)
    {
        _db.Rooms.Remove(await Get(id));
        await _db.SaveChangesAsync();
    }

    public async Task<Room> Get(ITuple id) 
        => await EagerLoad().SingleOrDefaultAsync(r => r.Number == (int)id[0]! && r.ScheduleId == (int)id[1]!) 
        ?? throw new InvalidOperationException($"No {nameof(Room)} object can be found with id: {id}.");

    public async Task<IEnumerable<Room>> GetAll() => await EagerLoad().ToListAsync();

    public async Task<Room> GetLazy(ITuple id)
        => await LazyLoad().SingleOrDefaultAsync(r => r.Number == (int)id[0]! && r.ScheduleId == (int)id[1]!) 
        ?? throw new InvalidOperationException($"No {nameof(Room)} object can be found with id: {id}.");


    public async Task<Room> Update(Room obj)
    {
        var room = await Get(Tuple.Create(obj.Number, obj.ScheduleId));
        room.Type = obj.Type;
        room.Number = obj.Number;
        room.Floor = obj.Floor;
        await _db.SaveChangesAsync();
        return room;
    }

    private IQueryable<Room> EagerLoad()
        => _db.Rooms.Include(r => r.Reservations)
                    .Include(r => r.Schedule);
    private IQueryable<Room> LazyLoad()
        => _db.Rooms;
}
