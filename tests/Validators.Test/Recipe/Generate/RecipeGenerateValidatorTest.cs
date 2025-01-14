using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Generate;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.Generate;

public class RecipeGenerateValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new RecipeGenerateValidator();
    var request = RecipeGenerateRequestBuilder.Build();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Error_More_Maximum_Ingredients()
  {
    var validator = new RecipeGenerateValidator();
    var request = RecipeGenerateRequestBuilder.Build(MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_RECIPE_GENERATE + 1);
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_INGREDIENTS_NUMBER));
  }

  [Fact]
  public void Error_Duplicated_Ingredients()
  {
    var validator = new RecipeGenerateValidator();
    var request = RecipeGenerateRequestBuilder.Build(count: 4);

    request.Ingredients.Add(request.Ingredients[0]);

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST));
  }

  [Fact]
  public void Error_Empty_Ingredient()
  {
    var validator = new RecipeGenerateValidator();
    var request = RecipeGenerateRequestBuilder.Build(count: 1);

    request.Ingredients.Add(string.Empty);

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
  }

  [Fact]
  public void Error_Ingredient_Not_Following_Pattern()
  {
    var validator = new RecipeGenerateValidator();
    var request = RecipeGenerateRequestBuilder.Build(count: 4);

    request.Ingredients.Add("This is a invalid ingredient because is too long");

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();

    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN));
  }
}
