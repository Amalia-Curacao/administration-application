using Creative.Api.Data;
using Creative.Api.Implementations.EntityFrameworkCore;
using Creative.Api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Controllers;

public class GuestController : Controller
{
	private readonly ICrud<Guest> _crud;
	private readonly IValidator<Guest> _validator;
	private readonly ValidationException PersonNotFound = new ValidationException($"{nameof(Guest)} not found in the database.");

	public GuestController(ScheduleDb db, IValidator<Guest> validator)
	{
		_crud = new Crud<Guest>(db);
		_validator = validator;
	}

	/// <summary> Api endpoint for getting all guests from a reservation in the database.</summary>
	/// <returns> Status 200 (OK) with all guests  from a reservation in the database.</returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Reservation.Id)}}}")]
	public async Task<ObjectResult> GetReservationGuests(int reservationId)
		=> Ok((await _crud.GetAll()).Where(g => g.ReservationId == reservationId));

	/// <summary> Api endpoint for getting a specific guest in the database.</summary>
	/// <returns> Status 200 (OK) with the guest with the given id.</returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Guest.Id)}}}")]
	public async Task<ObjectResult> Get(int id)
		=> Ok(await _crud.Get(new HashSet<Key>(new Key[] { new(nameof(Guest.Id), id) })));

	/// <summary> Api endpoint for creating guests in the database. </summary>
	/// <returns> Status 200 (OK) with the new guest, when the guest has been added. </returns>
	[HttpPost("[controller]/[action]")]
	public async Task<ObjectResult> Create(Guest person)
	{
		var result = _validator.Validate(person);
		return result.IsValid ? Ok((await _crud.Add(true, person))[0])
			: BadRequest(result.Errors);
	}

	/// <summary> Api endpoint for editing guests in the database. </summary>
	/// <returns> 
	/// Status 200 (OK) with the edited guest, when the guest has been edited.
	/// Status 400 (Bad request) with error message, when properties are invalid.
	/// </returns>
	[HttpPut("[controller]/[action]")]
	public async Task<ObjectResult> Edit(Guest person)
	{
		var result = _validator.Validate(person);
		return result.IsValid ? Ok(await _crud.Update(person))
			: BadRequest(result.Errors);
	}

	/// <summary> Api endpoint for deleting guests in the database. </summary>
	/// <returns> Status 200 (OK) and a boolean true, when the guest has been deleted. </returns>
	[HttpDelete($"[controller]/[action]/{{{nameof(Guest.Id)}}}")]
	public async Task<ObjectResult> Delete(Guest person)
		=> Ok(await _crud.Delete(person.GetPrimaryKey()));

}
