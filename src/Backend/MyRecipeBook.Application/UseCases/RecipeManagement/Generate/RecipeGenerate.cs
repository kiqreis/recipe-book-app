using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Generate;

public class RecipeGenerate(IRecipeGenerateAI generateAI) : IRecipeGenerate
{
  public async Task<RecipeGeneratedResponse> Execute(RecipeGeneratedRequest request)
  {
    Validate(request);

    var response = await generateAI.Generate(request.Ingredients);

    return new RecipeGeneratedResponse
    {
      Title = response.Title,
      Ingredients = response.Ingredients,
      CookingTime = (Communication.Enums.CookingTime)response.CookingTime,
      Instructions = response.Instructions.Select(i => new InstructionGeneratedResponse
      {
        Step = i.Step,
        Text = i.Text,
      }).ToList(),
      Difficulty = Communication.Enums.Difficulty.Low
    };
  }

  private static void Validate(RecipeGeneratedRequest request)
  {
    var result = new RecipeGenerateValidator().Validate(request);

    if (result.IsValid.IsFalse())
    {
      throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
  }
}
