using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Validators.Abstract;

namespace Scheduler.Controllers;

public class ReservationsController : Controller
{
    private readonly ICrud<Reservation> _crud;
    private readonly IValidator<Reservation> _validator;
    private readonly RelationshipValidator<Reservation> _relationshipValidator;

    public ReservationsController(ScheduleDb db, IValidator<Reservation> validator, RelationshipValidator<Reservation> relationshipValidator)
    {
        _crud = new Crud<Reservation>(db);
        _validator = validator;
        _relationshipValidator = relationshipValidator;
    }

    // GET: Reservations/Create
    public IActionResult Create()
    {
        if (TempData.IsNull($"{nameof(Room)}s") || TempData.IsNull(nameof(Room)) || TempData.IsNull($"{nameof(Reservation.CheckIn)}")) 
            return RedirectToAction(controllerName: "Rooms", actionName: "Index");

        var room = TempData.Peek<Room>(nameof(Room))!;
        ViewData[nameof(Room)] = room;
        ViewData[$"{nameof(Room)}s"] = TempData.Peek<Room[]>($"{nameof(Room)}s")!.Where(r => r.Type == room.Type).Select(r => r.RemoveRelations()).ToArray();

        return View();
    }

    // POST: Reservations/Create
    [HttpPost]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        var rooms = TempData.Peek<Room[]>($"{nameof(Room)}s");
        if (rooms is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");

        var room = rooms.FirstOrDefault(r => r.Number.Equals(reservation.RoomNumber));
        if (room is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");
        TempData.Put(nameof(Room), room);

        reservation.RoomScheduleId = room.ScheduleId;
        reservation.ScheduleId = room.ScheduleId;
        reservation.RoomNumber = room.Number;
        reservation.RoomType = room.Type;

        var result = _validator.Validate(reservation);
		if (!result.IsValid)
		{
            result.AddToModelState(ModelState);
			return View(reservation);
		}

        var successfullyAdded = await _crud.Add(reservation);

		if (!successfullyAdded)
        {
            ModelState.AddModelError(nameof(Reservation.RoomNumber), "Reservation already exists.");
			return View(reservation);
		}
        
        TempData.Put(nameof(Reservation), reservation.RemoveRelations());
        return RedirectToAction(controllerName: "People", actionName: "Create");
    }

    // GET: Reservations/Edit/1/5
    [HttpGet("[controller]/[action]/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var key = new Dictionary<string, object> { { nameof(Reservation.Id), id } };    
        var reservation = await _crud.GetNoCycle(key);
        if (reservation is null) return RedirectToAction(controllerName: "Reservations", actionName: "Create");
        TempData.Put(nameof(Reservation), reservation);
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(Reservation reservation)
    {
        if(TempData.IsNull(nameof(Reservation))) return RedirectToAction(controllerName: "Reservations", actionName: "Create");
		var oldReservation = TempData.Peek<Reservation>(nameof(Reservation))!;

		reservation.BookingSource = oldReservation.BookingSource;
        reservation.CheckIn ??= oldReservation.CheckIn;
        reservation.CheckOut ??= oldReservation.CheckOut;
        reservation.FlightArrivalTime ??= oldReservation.FlightArrivalTime;
        reservation.FlightDepartureTime ??= oldReservation.FlightDepartureTime;
        reservation.FlightArrivalNumber = oldReservation.FlightArrivalNumber;
        reservation.FlightArrivalNumber = oldReservation.FlightDepartureNumber;
        reservation.Id = oldReservation.Id;
        reservation.RoomNumber ??= oldReservation.RoomNumber;
        reservation.RoomScheduleId = oldReservation.RoomScheduleId;
        reservation.ScheduleId = oldReservation.ScheduleId;
        reservation.People = oldReservation.People;

        var result = _validator.Validate(reservation);
        if (!result.IsValid)
        {
			result.AddToModelState(ModelState);
			return View(reservation);
		}
        await _crud.Update(reservation);
        return RedirectToAction(controllerName: "Rooms", actionName: "Index");
    }

    // GET: Reservations/Delete/5
    public IActionResult Delete(int id)
    {
        _crud.Delete(new Dictionary<string, object> { { nameof(Reservation.Id), id! } });
        return RedirectToAction(controllerName: "Rooms", actionName: "Index");
    }
}
