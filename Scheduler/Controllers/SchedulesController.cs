using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;

namespace Scheduler.Controllers;

public class SchedulesController : Controller
{
    private readonly ICrud<Schedule> _crud;

    public SchedulesController(ICrud<Schedule> scheduleService)
    {
        _crud = scheduleService;
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
        TempData.Put("Schedule", await _crud.GetLazy(Tuple.Create(id)));
        return RedirectToAction(controllerName: "Rooms", actionName: "Index");
    }

    // GET: Schedules/Create
    public IActionResult Create()
    {
        _crud.Add(new Schedule());
        return RedirectToAction(nameof(Index));
    }

    // GET: Schedules/Delete/5
    public IActionResult Delete(int id)
    {
        _crud.Delete(Tuple.Create(id));
        return RedirectToAction(nameof(Index));
    }
}
