using FluentValidation;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Data.Validators;

public class RoomValidator : AbstractValidator<Room>
{
	public RoomValidator()
	{
		RuleFor(r => r.ScheduleId).NotNull().WithMessage($"Room requires a schedule.");
	}
}
