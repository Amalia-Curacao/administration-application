using Creative.Database.Data;
using FluentValidation;
using Roster.Data;
using Scheduler.Data.Models;
using Scheduler.Data.Validators;
using Scheduler.Data.Validators.Abstract;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AZURE-SQL-CONNECTIONSTRING");
var options = new SqlServerOptions() { ConnectionString = connectionString };
builder.Services.AddDbContext<ScheduleDb>(_ => ScheduleDb.Create(options));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMvc();

// Register validators
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
