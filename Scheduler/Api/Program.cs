using Creative.Database.Data;
using FluentValidation;
using Scheduler.Api.Data;
using Scheduler.Api.Data.Models;
using Scheduler.Api.Data.Validators;
using Scheduler.Api.Data.Validators.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Register database context
var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
    ?? throw new NullReferenceException($"The azure connection string cannot be null.");
var options = new SqlServerOptions() { ConnectionString = connectionString };
builder.Services.AddDbContext<ScheduleDb>(_ => ScheduleDb.Create(options));

// Register validators
builder.Services.AddScoped<IValidator<Guest>, PersonValidator>();
builder.Services.AddScoped<IValidator<Reservation>, ReservationValidator>();
builder.Services.AddScoped<IValidator<Room>, RoomValidator>();  
builder.Services.AddScoped<RelationshipValidator<Reservation>, ReservationRelationshipValidator>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllerRoute(
        name: "default",
        pattern: "swagger"
        );
}
else{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// TODO think about CORS
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Run();