﻿using Microsoft.EntityFrameworkCore;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services;

public class ReservationService : ICrud<Reservation>
{
    private readonly ScheduleDb _db;
    public ReservationService(ScheduleDb db)
    {
        _db = db;
    }
    public async void Add(Reservation obj)
    {
        _db.Reservations.Add(obj);
        await _db.SaveChangesAsync();
    }

    public async void Delete(ITuple id)
    {
        var toDelete = await Get(id);
        _db.Reservations.Remove(toDelete);
        _db.Person.RemoveRange(toDelete.People!);
        await _db.SaveChangesAsync();
    }

    public async Task<Reservation> Get(ITuple id) 
        => await EagerLoad().SingleOrDefaultAsync(r => r.Id == (int)id[0]!) 
        ?? throw new InvalidOperationException($"No {nameof(Reservation)} found with id: {id}.");

    public async Task<Reservation> GetNoCycle(ITuple id)
    {
        var reservation = await Get(id);
        reservation.Room!.Reservations = null;
        reservation.Room!.Schedule = null;
        reservation.Schedule!.Reservations = null;
        reservation.Schedule!.Rooms = null;
        foreach (var person in reservation.People!)
        {
            person.Reservation = null;
        }
        return reservation;
    }

    public async Task<IEnumerable<Reservation>> GetAll() 
        => await EagerLoad().ToListAsync();

    private IQueryable<Reservation> EagerLoad() 
        => _db.Reservations.Include(r => r.Room)
                           .Include(r => r.Schedule)
                           .Include(r => r.People);

    private IQueryable<Reservation> LazyLoad()
        => _db.Reservations;

    public async Task<Reservation> Update(Reservation obj)
    {
        var reservation = await Get(Tuple.Create(obj.Id));
        reservation.RoomNumber = obj.RoomNumber;
        // TODO change when People/Edit view is finished
        // reservation.People = obj.People;
        reservation.CheckIn = obj.CheckIn;
        reservation.CheckOut = obj.CheckOut;
        reservation.RoomType = obj.RoomType;
        reservation.FlightArrivalNumber = obj.FlightArrivalNumber;
        reservation.FlightArrivalTime = obj.FlightArrivalTime;
        reservation.FlightDepartureNumber = obj.FlightDepartureNumber;
        reservation.FlightDepartureTime = obj.FlightDepartureTime;
        reservation.BookingSource = obj.BookingSource;

        await _db.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation> GetLazy(ITuple id)
        => await LazyLoad().SingleOrDefaultAsync(r => r.Id == (int)id[0]!) 
        ?? throw new InvalidOperationException($"No {nameof(Reservation)} found with id: {id}.");
}