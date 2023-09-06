using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;

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
    public IActionResult Create([Bind("Type,Number,Floor")] Room room)
    {
        if (!ModelState.IsValid) return View(room);

        var schedule = TempData.Peek<Schedule>("Schedule");
        if (schedule is null) return RedirectToAction(controllerName: "Schedules", actionName: "Index");

        room.ScheduleId = schedule.Id;
        _crud.Add(room);

        return RedirectToAction(controllerName: "Rooms", actionName: "Index");
    }

    public async Task<IActionResult> AddReservation(int id) 
    {
        var schedule = TempData.Peek<Schedule>("Schedule");
        if (schedule is null) return RedirectToAction(controllerName: "Schedules", actionName: "Index");
        var room = await _crud.GetLazy((id, schedule.Id));
        if (room is null) return RedirectToAction(actionName: "Index");
        TempData.Put("Room", room);
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
