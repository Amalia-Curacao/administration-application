using Creative.Api.Data;
using Creative.Api.Implementations.EntityFrameworkCore;
using Creative.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduler.Api.Controllers;

public sealed class RoomsController : Controller
{
	private readonly ICrud<Room> _crud;
	private readonly ScheduleDb _db;
	private readonly ValidationException _roomNotFound = new("The room could not be located.");
	private readonly JsonSerializerOptions serializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		ReferenceHandler = ReferenceHandler.IgnoreCycles
	};

	public RoomsController(ScheduleDb db)
	{
		_crud = new Crud<Room>(db);
		_db = db;
	}

	/// <summary> Gets all the rooms in the database with a specific schedule id. </summary>
	/// <returns> Status 200 (OK) with the <see cref="Room"/>s, when the <see cref="Room"/>s have been found. </returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Reservation.ScheduleId)}}}")]
	public async Task<ObjectResult> Get([FromRoute] int ScheduleId)	
		=> Ok(JsonSerializer.Serialize(await _db.Rooms.Include(r => r.Reservations).Where(r => r.ScheduleId == ScheduleId).ToListAsync(), serializerOptions));
	

	/// <summary> Gets a specific room from the database. </summary>
	/// <returns> 
	/// Status 200 (OK) with the <see cref="Room"/>, when the <see cref="Room"/> has been found. 
	///	Status 404 (Not Found) with a <see cref="ValidationException"/>, when the <see cref="Room"/> could not be found.
	/// </returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Room.Number)}}}/{{{nameof(Room.ScheduleId)}}}")]
	public async Task<ObjectResult> Get([FromRoute] int Number, [FromRoute] int ScheduleId)
	{
		var room = await _crud.Get(new HashSet<Key>(new Key[] { new(nameof(Room.Number), Number), new(nameof(Room.ScheduleId), ScheduleId) }));
		return room is null ? NotFound(_roomNotFound) : Ok(room);
	}

	/// <summary> Creates a <see cref="Room"/>. </summary>
	/// <returns> Status 200 (OK) with the new <see cref="Room"/>, when the <see cref="Room"/> has been added. </returns>
	[HttpPost($"[controller]/[action]")]
	public async Task<ObjectResult> Create([FromBody] Room room)
		=> Ok((await _crud.Add(false, room))[0]);

	/// <summary> Updates a <see cref="Room"/>. </summary>
	/// <returns> Status 200 (OK) with the updated <see cref="Room"/>, when the <see cref="Room"/> has been updated. </returns>
	[HttpPut($"[controller]/[action]")]
	public async Task<ObjectResult> Edit([FromBody] Room room)
		=> Ok(await _crud.Update(room));

	/// <summary> Deletes a <see cref="Room"/>. </summary>
	/// <returns> Status 200 (OK) with the deleted <see cref="Room"/>, when the <see cref="Room"/> has been deleted. </returns>
	[HttpDelete($"[controller]/[action]/{{{nameof(Room.Number)}}}/{{{nameof(Room.ScheduleId)}}}")]
	public async Task<ObjectResult> Delete([FromRoute] int Number, [FromRoute] int ScheduleId)
		=> Ok(await _crud.Delete(new HashSet<Key>(new Key[] { new(nameof(Room.Number), Number), new(nameof(Room.ScheduleId), ScheduleId) })));
}
