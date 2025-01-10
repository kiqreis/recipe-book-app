using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Generate;

public class RecipeGenerateValidator : AbstractValidator<RecipeGeneratedRequest>
{
  public RecipeGenerateValidator()
  {
    var maximum_ingredients_number = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_RECIPE_GENERATE;

    RuleFor(request => request.Ingredients.Count).InclusiveBetween(1, maximum_ingredients_number)
      .WithMessage(ResourceMessagesException.INVALID_INGREDIENTS_NUMBER);

    RuleFor(request => request.Ingredients).Must(ingredients => ingredients.Count == ingredients.Distinct().Count())
      .WithMessage(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);

    RuleFor(request => request.Ingredients).ForEach(rule =>
    {
      rule.Custom((value, context) =>
      {
        if (string.IsNullOrWhiteSpace(value))
        {
          context.AddFailure(string.Empty, ResourceMessagesException.INGREDIENT_EMPTY);
          return;
        }

        if (value.Count(i => i == ' ') > 3 || value.Count(i => i == '/') > 1)
        {
          context.AddFailure(string.Empty, ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
          return;
        }
      });
    });  
  }
}
