using Creative.Api.Data;
using Creative.Api.Implementations.EntityFrameworkCore;
using Creative.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Controllers;

public sealed class RoomsController : Controller
{
	private readonly ICrud<Room> _crud;

	public RoomsController(ScheduleDb db)
		=> _crud = new Crud<Room>(db);

	/// <summary> Gets all the rooms in the database with a specific schedule id. </summary>
	/// <returns> Status 200 (OK) with the <see cref="Room"/>s, when the <see cref="Room"/>s have been found. </returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Reservation.ScheduleId)}}}")]
	public async Task<ObjectResult> Get(int scheduleId)
		=> Ok((await _crud.GetAll()).Where(r => r.ScheduleId == scheduleId));

	/// <summary> Gets a specific room from the database. </summary>
	/// <returns> Status 200 (OK) with the <see cref="Room"/>, when the <see cref="Room"/> has been found. </returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Room.Number)}}}/{{{nameof(Room.ScheduleId)}}}")]
	public async Task<ObjectResult> Get(int roomNumber, int scheduleId)
		=> Ok(await _crud.Get(new HashSet<Key>(new Key[] { new(nameof(Room.Number), roomNumber), new(nameof(Room.ScheduleId), scheduleId) })));

	/// <summary> Creates a <see cref="Room"/>. </summary>
	/// <returns> Status 200 (OK) with the new <see cref="Room"/>, when the <see cref="Room"/> has been added. </returns>
	[HttpPost($"[controller]/[action]")]
	public async Task<ObjectResult> Create(Room room)
		=> Ok((await _crud.Add(false, room))[0]);

	/// <summary> Updates a <see cref="Room"/>. </summary>
	/// <returns> Status 200 (OK) with the updated <see cref="Room"/>, when the <see cref="Room"/> has been updated. </returns>
	[HttpPut($"[controller]/[action]")]
	public async Task<ObjectResult> Edit(Room room)
		=> Ok(await _crud.Update(room));

	/// <summary> Deletes a <see cref="Room"/>. </summary>
	/// <returns> Status 200 (OK) with the deleted <see cref="Room"/>, when the <see cref="Room"/> has been deleted. </returns>
	[HttpDelete($"[controller]/[action]/{{{nameof(Room.Number)}}}/{{{nameof(Room.ScheduleId)}}}")]
	public async Task<ObjectResult> Delete(int roomNumber, int scheduleId)
		=> Ok(await _crud.Delete(new HashSet<Key>(new Key[] { new(nameof(Room.Number), roomNumber), new(nameof(Room.ScheduleId), scheduleId) })));
}
