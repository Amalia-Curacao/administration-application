using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Roster.Data;
using Scheduler.Data.Models;

namespace Scheduler.Controllers
{
	public class PeopleController : Controller
    {
        private readonly ICrud<Person> _crud;
        private readonly IValidator<Person> _validator;

		public PeopleController(ScheduleDb db, IValidator<Person> validator)
        {
            _crud = new Crud<Person>(db);
            _validator = validator;
        }

        // GET: People/Create
        public IActionResult Create()
            => TempData.IsNull(nameof(Reservation)) 
            ? RedirectToAction(controllerName: "Reservations", actionName: "Create") 
            : View();

        // POST: People/Create
        [HttpPost]
        public async Task<IActionResult> Create(Person person)
        {
            if(TempData.IsNull(nameof(Reservation))) return RedirectToAction(controllerName: "Rooms", actionName: "Index");
			var reservation = TempData.Peek<Reservation>(nameof(Reservation))!;

            person.ReservationId = reservation.Id;

            var result = _validator.Validate(person);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
				return View(person);
			}

			ViewData["CreationSuccessful"] = await _crud.Add(person);
            if ((bool)ViewData["CreationSuccessful"]!)
            {
                var numberOfPeople = reservation.People!.Count + 1;
                TempData["Number of People"] = numberOfPeople;
                if (numberOfPeople >= 2)
                {
                    TempData.Remove("Number of People");
					return RedirectToAction(controllerName: "Rooms", actionName: "Index");
				}
			}

            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var person = await _crud.GetNoCycle(new Dictionary<string, object> { { nameof(Schedule.Id), id! } });
            TempData.Put(nameof(Person), person);
            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Person person)
		{
			if (TempData.IsNull(nameof(Person))) return RedirectToAction(controllerName: "Reservations", actionName: "Edit");
			var oldPerson = TempData.Get<Person>(nameof(Person))!;

            oldPerson.FirstName = person.FirstName;
            oldPerson.LastName = person.LastName;
            oldPerson.Age = person.Age;
            oldPerson.Note = person.Note;
            oldPerson.Prefix = person.Prefix;

            await _crud.Update(oldPerson);
            return RedirectToAction(controllerName: "Reservations", actionName: "Edit", routeValues: new {id = oldPerson.ReservationId });
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var reservationId = (await _crud.GetLazy(new Dictionary<string, object> { { nameof(Person.Id), id! } })).ReservationId;
            _crud.Delete(new Dictionary<string, object> { { nameof(Person.Id), id! } });
            return RedirectToAction("Edit", "Reservations", new {id = reservationId});
        }
    }
}
