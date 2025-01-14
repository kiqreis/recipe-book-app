using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Services.OpenAI;

namespace CommonTestsUtilities.OpenAI;

public class RecipeGenerateAIBuilder
{
  public static IRecipeGenerateAI Build(RecipeGeneratedDto dto)
  {
    var mock = new Mock<IRecipeGenerateAI>();

    mock.Setup(service => service.Generate(It.IsAny<List<string>>())).ReturnsAsync(dto);

    return mock.Object;
  }
}
