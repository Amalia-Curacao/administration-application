using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;

namespace Scheduler.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ICrud<Reservation> _crud;

        public ReservationsController(ICrud<Reservation> crud)
        {
            _crud = crud;
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            if (TempData.Peek<Room>("Room") is null || TempData.Peek("CheckIn") is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");
            ViewData["CheckIn"] = TempData.Get<DateOnly>("CheckIn");
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            var room = TempData.Peek<Room>("Room");
            if (room is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");

            //TODO - Fix this, model state is never valid.
            // if (!ModelState.IsValid) return View(reservation);

            reservation.RoomScheduleId = room.ScheduleId;
            reservation.ScheduleId = room.ScheduleId;
            reservation.RoomNumber = room.Number;
            reservation.RoomType = room.Type;

            _crud.Add(reservation);

            TempData.Put("Reservation", reservation);
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
        public async Task<IActionResult> Edit([Bind("CheckIn, CheckOut, RoomNumber, BookingSource, FlightArrivalNumber, FlightDepartureNumber")] Reservation reservation)
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
            reservation.RoomType = oldReservation.RoomType;
            reservation.ScheduleId = oldReservation.ScheduleId;


            // TODO - Change this when adding people.
            reservation.People = oldReservation.People;
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
}
