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

  public RecipeRepositoryBuilder GetById(User user, Recipe? recipe)
  {
    if (recipe != null)
    {
      _repository.Setup(repository => repository.GetById(user, recipe.Id)).ReturnsAsync(recipe);
    }

    return this;
  }

  public RecipeRepositoryBuilder GetByIdUpdate(User user, Recipe? recipe)
  {
    if (recipe != null)
    {
      _repository.Setup(repository => repository.GetById(user, recipe.Id)).ReturnsAsync(recipe);
    }

    return this;
  }

  public RecipeRepositoryBuilder GetForDashboard(User user, IList<Recipe> recipes)
  {
    _repository.Setup(repository => repository.GetForDashboard(user)).ReturnsAsync(recipes);

    return this;
  }
}
