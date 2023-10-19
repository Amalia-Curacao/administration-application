using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Roster.Data;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Controllers;

public class SchedulesController : Controller
{
	private readonly ICrud<Schedule> _crud;

	public SchedulesController(ScheduleDb db)
	{
		_crud = new Crud<Schedule>(db);
	}

	// Tested
	public async Task<IActionResult> Index() 
		=> View(await _crud.GetAllNoCycle());

	// Tested
	[HttpGet($"[controller]/Page/[action]/{{{nameof(Schedule.Id)}}}")]
	public async Task<IActionResult> Details(int id)
	{
		var schedule = await _crud.Get(new Dictionary<string, object>() { { "Id", id } });
		return View(schedule);
	}

	// Tested
	[HttpGet($"[controller]/[action]/{{{nameof(Schedule.Name)}}}")]
	public async Task<IActionResult> Create(string name)
	{
		await _crud.Add(new Schedule() { Name = name });
		return RedirectToAction(nameof(Index));
	}

    // Tested
    [HttpDelete($"[controller]/[action]/{{{nameof(Schedule.Id)}}}")]
	public async Task<IActionResult> Delete(int id)
		=> await Delete(new Schedule() { Id = id});

    // Tested
    [HttpDelete("[controller]/[action]")]
	public async Task<IActionResult> Delete(Schedule schedule)
	{
        await _crud.Delete(schedule);
		return View(nameof(Index), await _crud.GetAll());
	}
}
