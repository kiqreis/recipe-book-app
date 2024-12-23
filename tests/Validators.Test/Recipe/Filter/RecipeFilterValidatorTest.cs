using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Filter;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.Filter;

public class RecipeFilterValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new FilterRecipeValidator();
    var request = RecipeFilterRequestBuilder.Build();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Error_Invalid_Cooking_Time()
  {
    var validator = new FilterRecipeValidator();
    var request = RecipeFilterRequestBuilder.Build();

    request.CookingTimes.Add((CookingTime)100);

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
  }

  [Fact]
  public void Error_Invalid_Difficulty()
  {
    var validator = new FilterRecipeValidator();
    var request = RecipeFilterRequestBuilder.Build();

    request.Difficulties.Add((Difficulty)100);

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
  }

  [Fact]
  public void Error_Invalid_DishType()
  {
    var validator = new FilterRecipeValidator();
    var request = RecipeFilterRequestBuilder.Build();

    request.DishTypes.Add((DishType)100);

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
  }
}
