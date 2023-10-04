using Creative.Api.Implementations.Entity_Framework;
using Creative.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

    // GET: Rooms
    public async Task<IActionResult> Index()
    {
        TempData.Remove(nameof(Room));
        TempData.Remove($"{nameof(Room)}s");

        if (TempData.IsNull(nameof(Schedule)))
        {
			return RedirectToAction(controllerName: "Schedules", actionName: "Index");
		}

        var all = await _crud.GetAll();
        var fromSchedule = all.Where(r => r.ScheduleId == TempData.Peek<Schedule>(nameof(Schedule))!.Id);
		var rooms = (await _crud.GetAll()).Where(r => r.ScheduleId == TempData.Peek<Schedule>(nameof(Schedule))!.Id);
		return View(rooms);

	}

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


	[HttpGet("[controller]/[action]/{id}/{checkIn}")]
	public async Task<IActionResult> AddReservation([FromRoute]int id, [FromRoute]DateOnly checkIn)
	{
        if (TempData.IsNull(nameof(Schedule)))
        {
            return RedirectToAction(controllerName: "Schedules", actionName: "Index");
        }

		var schedule = TempData.Peek<Schedule>(nameof(Schedule))!;
		var roomPrimaryKey = new Dictionary<string, object> { { nameof(Room.Number), id }, { nameof(Room.ScheduleId), schedule.Id! } };
		var room = await _crud.GetNoCycle(roomPrimaryKey);

		TempData.Put($"{nameof(Room)}s", await _crud.GetAllNoCycle());
        TempData.Put(nameof(Room), room);
        TempData.Put($"{nameof(Reservation.CheckIn)}", checkIn);

        return RedirectToAction(controllerName: "Reservations", actionName: "Create");
    }

	// GET: Rooms/Delete/5
	public IActionResult Delete(int id)
    {
        var schedule = TempData.Peek<Schedule>(nameof(Schedule));
        if (schedule is null) return RedirectToAction(controllerName: "Schedules", actionName: "Index");
        var roomPrimaryKey = new Dictionary<string, object> { { nameof(Room.Number), id }, { nameof(Room.ScheduleId), schedule.Id! } };
        _crud.Delete(roomPrimaryKey);
        return RedirectToAction(nameof(Index));
    }
}
