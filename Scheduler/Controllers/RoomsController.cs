using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Extensions;
using Scheduler.Data.Models;

namespace Scheduler.Controllers;

public sealed class RoomsController : Controller
{
    private readonly ICrud<Room> _crud;

    public RoomsController(ICrud<Room> crud)
    {
        _crud = crud;
    }

    // GET: Rooms
    public async Task<IActionResult> Index()
    {
        TempData.Remove("Room");
        TempData.Remove("Rooms");
        var schedule = TempData.Peek<Schedule>("Schedule");
        if (schedule is not null)
        {
            return View((await _crud.GetAll()).Where(r => r.ScheduleId == schedule.Id));
        }
        return RedirectToAction(controllerName: "Schedules", actionName: "Index");
    }

    // GET: Rooms/Create
    public IActionResult Create() => View();

    // POST: Rooms/Create
    [HttpPost]
    public async Task<IActionResult> Create(Room room)
    {
        if (!ModelState.IsValid) return View(room);

        var schedule = TempData.Peek<Schedule>("Schedule");
        if (schedule is null) return RedirectToAction(controllerName: "Schedules", actionName: "Index");

        room.ScheduleId = schedule.Id;
        await _crud.Add(room);

        return RedirectToAction(controllerName: "Rooms", actionName: "Index");
    }


	[HttpGet("[controller]/[action]/{id}/{checkIn}")]
	public async Task<IActionResult> AddReservation([FromRoute]int id, [FromRoute]DateOnly checkIn) 
    {
        var schedule = TempData.Peek<Schedule>("Schedule");
        if (schedule is null) return RedirectToAction(controllerName: "Schedules", actionName: "Index");
        var room = await _crud.GetNoCycle((id, schedule.Id));
        if (room is null) return RedirectToAction(actionName: "Index");

		TempData.Put("Rooms", await _crud.GetAllNoCycle().ToListAsync());
        TempData.Put("Room", room);
        TempData.Put("CheckIn", checkIn);
        return RedirectToAction(controllerName: "Reservations", actionName: "Create");
    }

	// GET: Rooms/Delete/5
	public IActionResult Delete(int id)
    {
        var schedule = TempData.Peek<Schedule>("Schedule");
        if (schedule is null) return RedirectToAction(controllerName: "Schedules", actionName: "Index");
        _crud.Delete((id, schedule.Id));
        return RedirectToAction(nameof(Index));
    }
}
