using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.RecipeRepository;

namespace CommonTestsUtilities.Repositories;

public class RecipeRepositoryBuilder
{
  private readonly Mock<IRecipeRepository> _repository;

  public RecipeRepositoryBuilder() => _repository = new Mock<IRecipeRepository>();

  public IRecipeRepository Build() => _repository.Object;

  public RecipeRepositoryBuilder Filter(User user, IList<Recipe> recipes)
  {
    _repository.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipeDto>())).ReturnsAsync(recipes);

    return this;
  }
}
