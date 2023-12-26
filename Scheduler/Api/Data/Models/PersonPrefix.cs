using System.Text.Json.Serialization;

namespace Scheduler.Api.Data.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]    
public enum PersonPrefix
{
    Unknown = 0,
    Mr = 1,
    Mrs = 2,
    Ms = 3,
    Other = 4
}
