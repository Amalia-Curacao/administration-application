using System.ComponentModel.DataAnnotations;

namespace Scheduler.Api.Data.Models;

public enum BookingSource
{
    [Display(Name = "None")]
    None = 0,

    [Display(Name = "TUI")]
    Tui = 1,

    [Display(Name = "Booking.com")]
    BookingDotCom = 2,

    [Display(Name = "Expedia")]
    Expedia = 3,

    [Display(Name = "Airbnb")]
    Airbnb = 4,

    [Display(Name = "Direct")]
    Direct = 5,
}
