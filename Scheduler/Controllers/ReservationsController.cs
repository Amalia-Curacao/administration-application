using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;
using Scheduler.Data.Validators.Abstract;

namespace Scheduler.Controllers;

public class ReservationsController : Controller
{
    private readonly ICrud<Reservation> _crud;
    private readonly IValidator<Reservation> _validator;
    private readonly RelationshipValidator<Reservation> _relationshipValidator;

    public ReservationsController(ICrud<Reservation> crud, IValidator<Reservation> validator, RelationshipValidator<Reservation> relationshipValidator)
    {
        _crud = crud;
        _validator = validator;
        _relationshipValidator = relationshipValidator;
    }

    // GET: Reservations/Create
    public IActionResult Create()
    {
        if (TempData.IsNull("Rooms") || TempData.IsNull("Room") || TempData.IsNull("CheckIn")) 
            return RedirectToAction(controllerName: "Rooms", actionName: "Index");
        TempData.Put("ViewRoom", TempData.Peek<Room>("Room")!.RemoveRelations());
        TempData.Put("ViewRooms", GetRooms(TempData.Peek<Room>("Room")!.Type!.Value).Select(r => r.RemoveRelations()));
        return View();
    }

    private Room[] GetRooms(RoomType type)
        => TempData.Peek<Room[]>("Rooms")!.Where(r => r.Type.Equals(type)).ToArray();

    // POST: Reservations/Create
    [HttpPost]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        var rooms = TempData.Peek<Room[]>("Rooms");
        if (rooms is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");

        var room = rooms.FirstOrDefault(r => r.Number.Equals(reservation.RoomNumber));
        if (room is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");
        TempData.Put("Room", room);

        reservation.RoomScheduleId = room.ScheduleId;
        reservation.ScheduleId = room.ScheduleId;
        reservation.RoomNumber = room.Number;

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
        
        TempData.Put("Reservation", reservation.RemoveRelations());
        return RedirectToAction(controllerName: "People", actionName: "Create");
    }

    // GET: Reservations/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var reservation = await _crud.GetNoCycle(Tuple.Create(id));
        if (reservation is null) return RedirectToAction(controllerName: "Reservations", actionName: "Create");
        TempData.Put("Reservation", reservation);
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(Reservation reservation)
    {
        var oldReservation = TempData.Peek<Reservation>("Reservation");
        if(oldReservation is null) return RedirectToAction(controllerName: "Reservations", actionName: "Create");
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
        _crud.Delete(Tuple.Create(id));
        return RedirectToAction(controllerName: "Rooms", actionName: "Index");
    }
}
