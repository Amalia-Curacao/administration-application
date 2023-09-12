using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;

namespace Scheduler.Controllers
{
	public class PeopleController : Controller
    {
        private readonly ICrud<Person> _crud;

        public PeopleController(ICrud<Person> crud)
        {
            _crud = crud;
        }

        // GET: People/Create
        public IActionResult Create()
        {
            if (TempData.Peek<Reservation>("Reservation") is null) return RedirectToAction(controllerName: "Reservations", actionName: "Create");
            return View();
        }

        // POST: People/Create
        [HttpPost]
        public IActionResult Create([Bind("FirstName, LastName, Age, Note, Prefix")] Person person)
        {
            var reservation = TempData.Peek<Reservation>("Reservation");
            if (reservation is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");

            person.ReservationId = reservation.Id;

            _crud.Add(person);
            if (TempData["Number of People"] is not null)
            {
                if ((int)TempData["Number of People"]! == 1)
                {
                    return RedirectToAction(controllerName: "Rooms", actionName: "Index");
                }
            }

            ViewData["CreationSuccessful"] = true;
            TempData["Number of People"] = reservation.People!.Count() + 1;
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var person = await _crud.GetNoCycle(Tuple.Create(id));
            if (person is null) return RedirectToAction(controllerName: "Reservations", actionName: "Edit");
            TempData.Put("Person", person);
            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("FirstName, LastName, Age, Note, Prefix")] Person person)
        {
            var oldPerson = TempData.Get<Person>("Person");
            if (oldPerson is null) return RedirectToAction(controllerName: "Reservations", actionName: "Edit");

            oldPerson.FirstName = person.FirstName;
            oldPerson.LastName = person.LastName;
            oldPerson.Age = person.Age;
            oldPerson.Note = person.Note;
            oldPerson.Prefix = person.Prefix;

            await _crud.Update(oldPerson);
            return RedirectToAction(controllerName: "Reservations", actionName: "Edit", routeValues: oldPerson.ReservationId);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var reservationId = (await _crud.GetLazy(Tuple.Create(id))).ReservationId;
            _crud.Delete(Tuple.Create(id));
            return RedirectToAction("Edit", "Reservations", new {id = reservationId});
        }
    }
}
