using Creative.Api.Data;
using Creative.Api.Implementations.EntityFrameworkCore;
using Creative.Api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Controllers;

public class SchedulesController : Controller
{
	private readonly ICrud<Schedule> _crud;
	private readonly ValidationException NotLocated = new("The schedule could not be located.");

	public SchedulesController(ScheduleDb db)
		=> _crud = new Crud<Schedule>(db);

	/// <summary> Api endpoint for getting all schedules in the database.</summary>
	/// <returns> Status 200 (OK) with all schedules in the database.</returns>
	[HttpGet("[controller]/[action]")]
	public async Task<ObjectResult> Get()
		=> Ok(await _crud.GetAll());

	/// <summary> Api endpoint for getting a schedule by id.</summary>
	/// <returns> Status 200 (OK) with the schedule with the given id.</returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Schedule.Id)}}}")]
	public async Task<ObjectResult> Get([FromRoute] int Id)
	{
		var schedule = await _crud.TryGet(new HashSet<Key>(new Key[] { new(nameof(Schedule.Id), Id) }));
		return schedule is null ? BadRequest(NotLocated) : Ok(schedule);
	}

	/// <summary> Api endpoint for creating schedules in the database. </summary>
	/// <returns> Status 200 (OK) with the new schedule, when the schedule has been added.</returns>
	[HttpGet($"[controller]/[action]/{{{nameof(Schedule.Name)}}}")]
	public async Task<ObjectResult> Create([FromRoute] string Name)
		=> Ok((await _crud.Add(true, new Schedule() { Name = Name })).Single());
	
	/// <summary> Api endpoint for editing schedules in the database. </summary>
	/// <returns> Status 200 (OK) with the edited schedule, when the schedule has been edited.</returns>
	[HttpPut($"[controller]/[action]")]
	public async Task<ObjectResult> Edit([FromBody]Schedule schedule)
		=> Ok(await _crud.Update(schedule));

	/// <summary> Api endpoint for deleting schedules in the database. </summary>
	/// <returns> Status 200 (OK) with the deleted schedule, when the schedule has been deleted.</returns>
	[HttpDelete($"[controller]/[action]/{{{nameof(Schedule.Id)}}}")]
	public async Task<ObjectResult> Delete([FromRoute] int Id)
		=> Ok(await _crud.Delete(new HashSet<Key>(new Key[] { new(nameof(Schedule.Id), Id) })));
}
