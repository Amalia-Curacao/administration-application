using FluentValidation;

namespace Scheduler.Api.Data.Validators.Abstract;

/// <summary> Validates relations of entities. </summary>
public abstract class RelationshipValidator<T> : AbstractValidator<T>
{
}
