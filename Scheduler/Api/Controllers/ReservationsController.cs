using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduler.Api.Controllers;

public class ReservationsController : Controller
{
	private readonly ICrud<Reservation> _crud;
	private readonly IValidator<Reservation> _validator;

	private static readonly ValidationFailure RoomFull = new ValidationFailure(nameof(Reservation.RoomNumber), "Reservation could not be added, because it was overlapping with an existing reservation.");
	private static readonly ValidationException ReservationNotFound = new ValidationException("Reservation could not be found");
	public ReservationsController(ScheduleDb db, IValidator<Reservation> validator)
	{
		_crud = new Crud<Reservation>(db);
		_validator = validator;
	}

	/// <summary> Api endpoint for creating reservations in the database. </summary>
	/// <returns> 
	/// An HTTP Status: 200 (OK) with the new reservation, when the reservation has been added.
	/// An HTTP Status: 400 (Bad request) with error message, when the reservation overlaps another.
	/// A new View with validation error, for fields that do not comply with the validation rules.
	/// </returns>
	[HttpPost("[controller]/[action]")]
	public async Task<IActionResult> Create(Reservation reservation)
	{
		// Validates properties
		var result = _validator.Validate(reservation);
		if (!result.IsValid)
		{
			result.AddToModelState(ModelState);
			return BadRequest(result);
		}

		// Adds reservation to databases
		await _crud.Add(reservation);

		// Checks if reservation fits in the room, if not delete it from database.
		reservation = await _crud.Get(reservation.GetPrimaryKey());
		if (!reservation.Room!.CanFit(reservation))
		{
			await _crud.Delete(reservation);
			// Add error that shows room in not available.
			result.Errors.Add(RoomFull);
			return BadRequest(result);
		}
		var serializationOptions = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
		var json = JsonSerializer.Serialize(reservation, serializationOptions);
		return Ok(JsonSerializer.Deserialize<Reservation>(json, serializationOptions));
	}


	// TODO: test
	[HttpPut("[controller]/[action]")]
	public async Task<IActionResult> Edit(Reservation reservation)
	{
		try
		{
			await _crud.Get(reservation.GetPrimaryKey());
        }
		catch (Exception)
		{
			return BadRequest(ReservationNotFound);
		}

		// Validates properties.
		var result = _validator.Validate(reservation);
		if (!result.IsValid)
		{
			return BadRequest(result);
		}

		reservation = await _crud.Update(reservation);
		if (!reservation.Room!.CanFit(reservation))
		{
			await _crud.Delete(reservation);
            result.Errors.Add(RoomFull);
            return BadRequest(result);
		}

		return Ok(reservation);
	}

	[HttpDelete("[controller]/[action]")]
	public async Task<IActionResult> Delete(Reservation reservation)
	{
		try
		{
            reservation = await _crud.Get(reservation.GetPrimaryKey());
            await _crud.Delete(reservation);
            return Ok();
        }
		catch (Exception e)
		{
			return BadRequest(e);
		}
	}
}
 