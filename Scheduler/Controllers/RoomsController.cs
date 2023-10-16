﻿using Creative.Api.Implementations.Entity_Framework;
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

	// TODO: test
	[HttpPost($"[controller]/[action]")]
	public async Task<IActionResult> Index(Schedule schedule)
		=> View((await _crud.GetAll()).Where(room => room.ScheduleId == schedule.Id));

	// TODO: test
	[HttpPost("[controller]/[action]")]
	public IActionResult Create(Schedule schedule) 
		=> View(new Room() { Floor = null, Number = null, Type = null, ScheduleId = (int)schedule.Id! });

	// TODO: test
	[HttpPost("[controller]/[action]")]
	public async Task<IActionResult> Create(Room room)
	{
		if (!ModelState.IsValid) return View(room);
		if (!await _crud.Add(room)) return View(room);
		var key = new Dictionary<string, object>() { { nameof(Room.Number), room.Number! }, { nameof(Room.ScheduleId), room.ScheduleId } };
		return RedirectToAction(nameof(Index), new { (await _crud.Get(key)).Schedule });
	}

	[HttpDelete("[controller]/[action]")]
	public async Task<IActionResult> Delete(Room room)
	{
		await _crud.Delete(room);
		return RedirectToAction(nameof(Index));
	}
}
