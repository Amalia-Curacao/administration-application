using Creative.Api.Data;
using Creative.Api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduler.Api.Controllers
{
	public class PeopleController : Controller
	{
		private readonly ICrud<Person> _crud;
		private readonly IValidator<Person> _validator;
		private readonly JsonSerializerOptions SerialaztionOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
		private readonly ValidationException PersonNotFound = new ValidationException($"{nameof(Person)} not found in the database.");

		public PeopleController(ScheduleDb db, IValidator<Person> validator)
		{
			_crud = new Crud<Person>(db);
			_validator = validator;
		}

		[HttpGet("[controller]/[action]/{id}")]
		public IActionResult Get(int id)
		{
			var person = _crud.Get(new HashSet<Key> { new Key(nameof(Person.Id), id) });
			if (person == null)
			{
				return BadRequest(PersonNotFound);
			}

			return Ok(JsonSerializer.Serialize(person, SerialaztionOptions));
		}

		// TODO: test
		[HttpPost("[controller]/[action]")]
		public async Task<IActionResult> Create(Person person)
		{
			var results = _validator.Validate(person);
			if (!results.IsValid)
			{
				return BadRequest(results);
			}

			await _crud.Add(person);

			person = _crud.Get(person.GetPrimaryKey());

			return Ok(JsonSerializer.Serialize(person, SerialaztionOptions));
		}

		// TODO: test
		[HttpPut("[controller]/[action]")]
		public async Task<IActionResult> Edit(Person person)
		{
			try
			{
				_ = await _crud.Get(person.GetPrimaryKey());
			}
			catch (Exception)
			{
				return BadRequest(PersonNotFound);
			}

			// Validates properties.
			var results = _validator.Validate(person);
			if (!results.IsValid)
			{
				return BadRequest(results);
			}

			person = await _crud.Update(person);

			return Ok(JsonSerializer.Serialize(person, SerialaztionOptions));
		}

		// TODO: test
		[HttpDelete("[controller]/[action]")]
		public async Task<IActionResult> Delete(Person person)
		{
			person = await _crud.Get(person.GetPrimaryKey());
			await _crud.Delete(person);
			return RedirectToAction(nameof(ReservationsController.Edit), "Reservations", person.Reservation);
		}
	}
}
