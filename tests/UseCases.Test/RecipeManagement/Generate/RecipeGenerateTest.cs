using CommonTestsUtilities.Dtos;
using CommonTestsUtilities.OpenAI;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Generate;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.RecipeManagement.Generate;

public class RecipeGenerateTest
{
  [Fact]
  public async Task Success()
  {
    var dto = RecipeGeneratedDtoBuilder.Build();
    var request = RecipeGenerateRequestBuilder.Build();
    var useCase = RecipeGenerate(dto);
    var result = await useCase.Execute(request);

    result.Should().NotBeNull();
    result.Title.Should().Be(dto.Title);
    result.CookingTime.Should().Be((MyRecipeBook.Communication.Enums.CookingTime)dto.CookingTime);
    result.Difficulty.Should().Be(MyRecipeBook.Communication.Enums.Difficulty.Low);
  }

  [Fact]
  public async Task Error_Duplicated_Ingredients()
  {
    var dto = RecipeGeneratedDtoBuilder.Build();
    var request = RecipeGenerateRequestBuilder.Build(count: 4);

    request.Ingredients.Add(request.Ingredients[0]);

    var useCase = RecipeGenerate(dto);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST));
  }

  private static RecipeGenerate RecipeGenerate(RecipeGeneratedDto dto)
  {
    var recipeGenerateAI = RecipeGenerateAIBuilder.Build(dto);

    return new RecipeGenerate(recipeGenerateAI);
  }
}
