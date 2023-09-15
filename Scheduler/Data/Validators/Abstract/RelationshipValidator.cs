using FluentValidation;

namespace Scheduler.Data.Validators.Abstract;

/// <summary> Validates relations of entities. </summary>
public abstract class RelationshipValidator<T> : AbstractValidator<T>
{
}
