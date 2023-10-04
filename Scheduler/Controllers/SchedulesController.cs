using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Roster.Data;
using Scheduler.Data.Models;

namespace Scheduler.Controllers;

public class SchedulesController : Controller
{
	private readonly ICrud<Schedule> _crud;

	public SchedulesController(ScheduleDb db)
	{
		_crud = new Crud<Schedule>(db);
	}

	// GET: Schedules
	public async Task<IActionResult> Index()
	{
		TempData.Clear();
		return View(await _crud.GetAll());
	}

	// GET: Schedules/Details/{id?}
	public async Task<IActionResult> Details(int id)
	{
		TempData.Put(nameof(Schedule), await _crud.Get(new Dictionary<string, object> { { nameof(Schedule.Id), id! } }));
		return RedirectToAction(controllerName: "Rooms", actionName: "Index");
	}

	// GET: Schedules/Create
	public async Task<IActionResult> Create()
	{
		await _crud.Add(new Schedule());
		return RedirectToAction(nameof(Index));
	}

	// GET: Schedules/Delete/5
	public IActionResult Delete(int id)
	{
		_crud.Delete(new Dictionary<string, object> { { nameof(Schedule.Id), id! } });
		return RedirectToAction(nameof(Index));
	}
}
