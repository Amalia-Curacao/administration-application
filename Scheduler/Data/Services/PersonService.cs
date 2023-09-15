using Microsoft.EntityFrameworkCore;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services;

public class PersonService : ICrud<Person>
{
	private readonly ScheduleDb _db;
	public PersonService(ScheduleDb db)
	{
		_db = db;
	}

	public async Task<bool> Add(Person obj)
	{
		await _db.Person.AddAsync(obj);
		await _db.SaveChangesAsync();
		return true;
	}

	public async void Delete(ITuple id)
	{
		var person = await Get(id);
		if (person is null) throw new InvalidOperationException($"Could not find person in the database with primary key: {id}.");
		_db.Person.Remove(person);
		await _db.SaveChangesAsync();
	}

	public async Task<Person> Get(ITuple id)
		=> await EagerLoad().SingleOrDefaultAsync(p => p.Id == (int)id[0]!) 
		?? throw new InvalidOperationException($"No {nameof(Person)} found with id: {id}.");

	public async Task<Person> GetNoCycle(ITuple id)
	{
        var person = await Get(id);
        person.Reservation!.People = null;
        return person;
    }

	private IQueryable<Person> EagerLoad()
		=> _db.Person.Include(p => p.Reservation);

	private IQueryable<Person> LazyLoad()
		=> _db.Person;

	public async Task<IEnumerable<Person>> GetAll()
		=> await EagerLoad().ToListAsync();

	public async Task<Person> GetLazy(ITuple id)
		=> await LazyLoad().SingleOrDefaultAsync(p => p.Id == (int)id[0]!) 
		?? throw new InvalidOperationException($"No {nameof(Person)} found with id: {id}.");

	public async Task<Person> Update(Person obj)
	{
		var person = await Get(Tuple.Create(obj.Id));
		person.FirstName = obj.FirstName;
		person.Age = obj.Age;
		person.Note = obj.Note;
		person.Prefix = obj.Prefix;
		await _db.SaveChangesAsync();
		return person;
	}

    public async IAsyncEnumerable<Person> GetAllNoCycle()
    {
        foreach (var person in await GetAll())
		{
            person.Reservation!.People = null;
			person.Reservation!.Schedule = null;
			person.Reservation!.Room = null;

			yield return person;
        }
    }
}
