using FluentValidation;
using Scheduler.Data.Models;

namespace Scheduler.Data.Validators;

public class RoomValidator : AbstractValidator<Room>
{
	public RoomValidator()
	{
		RuleFor(r => r.ScheduleId).NotNull().WithMessage($"Room requires a schedule.");
	}
}
