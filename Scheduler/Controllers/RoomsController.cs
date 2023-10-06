using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Roster.Data;
using Scheduler.Data.Models;

namespace Scheduler.Controllers;

public sealed class RoomsController : Controller
{
	private readonly ICrud<Room> _crud;

	public RoomsController(ScheduleDb db)
	{
		_crud = new Crud<Room>(db);
	}

	// TODO: test
	[HttpPost($"[controller]/[action]")]
	public async Task<IActionResult> Index(Schedule schedule)
		=> View((await _crud.GetAll()).Where(room => room.ScheduleId == schedule.Id));

	// GET: Rooms/Create
	public IActionResult Create() => View();

	// POST: Rooms/Create
	[HttpPost]
	public async Task<IActionResult> Create(Room room)
	{
		if (!ModelState.IsValid) return View(room);

		if (TempData.IsNull(nameof(Schedule))) return RedirectToAction(controllerName: "Schedules", actionName: "Index");

		var schedule = TempData.Peek<Schedule>(nameof(Schedule))!;
		room.ScheduleId = (int)schedule.Id!;
		await _crud.Add(room);

		return RedirectToAction(controllerName: "Rooms", actionName: "Index");
	}


	[HttpPost("[controller]/[action]")]
	public async Task<IActionResult> AddReservation([FromBody] JObject data)
	{
		if(data["room"] is null) return RedirectToAction(controllerName: "Rooms", actionName: "Index");

		var room = data["room"]!.ToObject<Room>();
		DateOnly? checkIn = data["checkIn"] is null ? null : data["checkIn"]!.ToObject<DateOnly>();

		TempData.Put($"{nameof(Room)}s", await _crud.GetAllNoCycle());
		TempData.Put(nameof(Room), room);
		TempData.Put($"{nameof(Reservation.CheckIn)}", checkIn);

		return RedirectToAction(controllerName: "Reservations", actionName: "Create");
	}

	[HttpDelete("[controller]/[action]/{scheduleId}/{number}")]
	public IActionResult Delete(int scheduleId, int number)
		=> Delete(new Room() { Number = number, ScheduleId = scheduleId, Floor = null, Type = null });

	[HttpDelete("[controller]/[action]")]
	public IActionResult Delete(Room room)
	{
		_crud.Delete(room);
		return RedirectToAction(controllerName: "Rooms", actionName: "Index");
	}
}
