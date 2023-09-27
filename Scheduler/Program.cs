using FluentValidation;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services;
using Scheduler.Data.Services.Interfaces;
using Scheduler.Data.Validators;
using Scheduler.Data.Validators.Abstract;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>(optional: false);
var connectionString = builder.Configuration["AZURE_SQL_CONNECTIONSTRING"] 
    ?? throw new ArgumentNullException("Connection string not found. Please add to local Secrets.json file under \"AZURE-SQL-CONNECTIONSTRING\".");
builder.Services.AddDbContext<ScheduleDb>(_ => new ScheduleDb(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICrud<Schedule>, ScheduleService>();
builder.Services.AddScoped<ICrud<Room>, RoomService>();
builder.Services.AddScoped<IRead<Room>, RoomService>();
builder.Services.AddScoped<ICrud<Reservation>, ReservationService>();
builder.Services.AddScoped<ICrud<Person>, PersonService>();

builder.Services.AddMvc();
builder.Services.AddScoped<IValidator<Person>, PersonValidator>();
builder.Services.AddScoped<IValidator<Reservation>, ReservationValidator>();
builder.Services.AddScoped<IValidator<Room>, RoomValidator>();  
builder.Services.AddScoped<RelationshipValidator<Reservation>, ReservationRelationshipValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Schedules}/{action=Details}/{id=1}"
    );

app.Run();
