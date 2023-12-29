using FluentValidation;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Data.Validators;

internal sealed class ReservationValidator : AbstractValidator<Reservation>
{
	public ReservationValidator()
	{
		RuleFor(reservation => reservation.CheckIn)
			.NotEmpty().WithMessage($"Check-in is required.");
		RuleFor(reservation => reservation.CheckOut)
			.NotEmpty().WithMessage($"Check out is required.");
		RuleFor(reservation => reservation.CheckIn)
			.LessThan(reservation => reservation.CheckOut).WithMessage($"Check-in must be before check out.");
		RuleFor(reservation => reservation.RoomNumber)
			.NotEmpty().WithMessage($"Room/apartment number is required.");
	}
}
