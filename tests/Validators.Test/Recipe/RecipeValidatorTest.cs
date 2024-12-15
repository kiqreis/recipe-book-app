using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;
using System.Globalization;

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

  [Fact]
  public void Success_When_DishTypes_Is_Empty()
  {
    var request = RecipeRequestBuilder.Build();

    request.DishTypes.Clear();

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Error_Invalid_DishTypes()
  {
    var request = RecipeRequestBuilder.Build();

    request.DishTypes.Add((DishType)100);

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
  }

  [Fact]
  public void Error_Empty_Ingredients()
  {
    var request = RecipeRequestBuilder.Build();

    request.Ingredients.Clear();

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
  }

  [Fact]
  public void Error_Empty_Instructions()
  {
    var request = RecipeRequestBuilder.Build();

    request.Instructions.Clear();

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
  }

  [Fact]
  public void Error_Empty_Value_Ingredients()
  {
    var request = RecipeRequestBuilder.Build();

    request.Ingredients.Add(string.Empty);

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
  }

  [Fact]
  public void Error_Same_Step_Instruction()
  {
    var request = RecipeRequestBuilder.Build();

    request.Instructions.First().Step = request.Instructions.Last().Step;

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
  }

  [Fact]
  public void Error_Negative_Step_Instruction()
  {
    var request = RecipeRequestBuilder.Build();

    request.Instructions.First().Step = -(new Random().Next(1, 100));

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP));
  }

  [Fact]
  public void Error_Empty_Value_Instruction()
  {
    var request = RecipeRequestBuilder.Build();

    request.Instructions.First().Text = string.Empty;

    var validator = new RecipeValidator();
    var result = validator.Validate(request);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
  }
}
