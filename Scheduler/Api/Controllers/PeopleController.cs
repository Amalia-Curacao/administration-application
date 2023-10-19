using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Roster.Data;
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

		// TODO: test
		[HttpPost("[controller]/Page/[action]")]
		public IActionResult Create(Person person, ValidationResult? validationResult)
		{
			validationResult?.AddToModelState(ModelState);
            return View(person);
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

			person = await _crud.Get(person.GetPrimaryKey());

			return Ok(JsonSerializer.Serialize(person, SerialaztionOptions));
		}

		// TODO: test
		[HttpPut($"[controller]/Page/{nameof(Edit)}")]
		public async Task<IActionResult> PageEdit(Person person, ValidationResult? validationResult)
		{
			if(validationResult is null)
			{
				var primaryKey = person.GetPrimaryKey();
				try
				{
					person = await _crud.Get(primaryKey);

                }
				catch (Exception)
				{
					return BadRequest(PersonNotFound);
				}
                return View(person);
            }
			else
			{
				validationResult.AddToModelState(ModelState);
				return View(person);
			}
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
