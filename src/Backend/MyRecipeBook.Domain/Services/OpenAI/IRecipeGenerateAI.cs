using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Services.OpenAI;

public interface IRecipeGenerateAI
{
  Task<RecipeGeneratedDto> Generate(IList<string> ingredients);
}
