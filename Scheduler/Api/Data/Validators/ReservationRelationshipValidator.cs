using FluentValidation;
using Scheduler.Api.Data.Models;
using Scheduler.Api.Data.Validators.Abstract;

namespace Scheduler.Api.Data.Validators;

internal class ReservationRelationshipValidator : RelationshipValidator<Reservation>
{
	public ReservationRelationshipValidator()
	{
		RuleFor(r => r.Guests).NotEmpty().WithMessage($"Reservation must have at least one person.");
		RuleFor(r => r.Schedule).NotNull().WithMessage($"Reservation must have a schedule.");
	}
}
