using FluentValidation;
using Scheduler.Data.Models;
using Scheduler.Data.Validators.Abstract;

namespace Scheduler.Data.Validators;

internal class ReservationRelationshipValidator : RelationshipValidator<Reservation>
{
	public ReservationRelationshipValidator()
	{
		RuleFor(r => r.People).NotEmpty().WithMessage($"Reservation must have at least one person.");
		RuleFor(r => r.Schedule).NotNull().WithMessage($"Reservation must have a schedule.");
	}
}
