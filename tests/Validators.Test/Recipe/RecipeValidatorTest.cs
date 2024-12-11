using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
  [Fact]
  public void Success()
  {
    var validator = new RecipeValidator();
    var request = RecipeRequestBuilder.Build();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Error_Cooking_Time_Not_Supported()
  {
    var validator = new RecipeValidator();
    var request = RecipeRequestBuilder.Build();

    request.CookingTime = (CookingTime?)100;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
  }

  [Fact]
  public void Error_Difficulty_Level_Not_Supported()
  {
    var validator = new RecipeValidator();
    var request = RecipeRequestBuilder.Build();

    request.Difficulty = (Difficulty?)100;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
  }

  [Fact]
  public void Error_Empty_Title()
  {
    var validator = new RecipeValidator();
    var request = RecipeRequestBuilder.Build();

    request.Title = string.Empty;

    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
  }

  [Fact]
  public void Success_When_Cooking_Time_Is_Null()
  {
    var validator = new RecipeValidator();
    var request = RecipeRequestBuilder.Build();

    request.CookingTime = null;

    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Success_When_Difficulty_Is_Null()
  {
    var validator = new RecipeValidator();
    var request = RecipeRequestBuilder.Build();

    request.Difficulty = null;

    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }
}
