using System.Text.Json.Serialization;

namespace Scheduler.Api.Data.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoomType
{
    None = 0,
    Apartment = 1,
    Room = 2,
}