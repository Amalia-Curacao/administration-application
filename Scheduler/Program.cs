using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Services;
using Scheduler.Data.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ScheduleDb>(builder => new ScheduleDb(builder.Options));
builder.Services.AddScoped<ICrud<Schedule>, ScheduleService>();
builder.Services.AddScoped<ICrud<Room>, RoomService>();
builder.Services.AddScoped<ICrud<Reservation>, ReservationService>();

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
    pattern: "{controller=Schedules}/{action=Index}/{id?}");

app.Run();
