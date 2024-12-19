using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Filter;

public class FilterRecipeValidator : AbstractValidator<RecipeFilterRequest>
{
  public FilterRecipeValidator()
  {
    RuleFor(recipe => recipe.CookingTimes).IsInEnum().WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
    RuleFor(recipe => recipe.Difficulties).IsInEnum().WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
    RuleFor(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
  }
}
