using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FluentValidation.Results;

internal static class ValidationResultExtensions
{

	/// <summary> This method is used to add the errors from a ValidationResult to the ModelState. </summary>
	public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
	{
		foreach (var error in result.Errors)
		{
			modelState.AddModelError(error.PropertyName, error.ErrorMessage);
		}
	}
}
