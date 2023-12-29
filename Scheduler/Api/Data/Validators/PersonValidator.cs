using FluentValidation;
using Scheduler.Api.Data.Models;

namespace Scheduler.Api.Data.Validators;

public class PersonValidator : AbstractValidator<Guest>
{
	public PersonValidator()
	{
		RuleFor(p => p.FirstName)
			.NotEmpty().WithMessage($"Person's first name is required.")
			.MaximumLength(50).WithMessage($"Person's first name can not be longer than 50 characters.");
		RuleFor(p => p.LastName)
			.NotEmpty().WithMessage($"Person's last name is required.")
			.MaximumLength(50).WithMessage($"Person's last name can not be longer than 50 characters.");
		RuleFor(p => p.Age)
			.NotEmpty().WithMessage($"Person's age is required.")
			.InclusiveBetween(0, 150).WithMessage($"Person's age needs to be between 0 and 150 years.");
		RuleFor(p => p.Prefix)
			.IsInEnum().WithMessage($"Person's prefix needs to match with one of the following \"Mr.\", \"Ms\", \"Mrs.\" or \"Other\".");
		RuleFor(p => p.ReservationId)
			.NotEmpty().WithMessage($"Person's reservation id is required.")
			.NotNull().WithMessage($"Person's reservation id is required.");
	}
}
