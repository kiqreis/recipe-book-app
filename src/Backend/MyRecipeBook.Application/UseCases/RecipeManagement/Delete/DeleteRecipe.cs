using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Delete;

public class DeleteRecipe(ILoggedUser _loggedUser, IRecipeRepository repository, IUnitOfWork unitOfWork) : IDeleteRecipe
{
  public async Task Execute(long recipeId)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await repository.GetById(loggedUser, recipeId);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    await repository.Delete(recipeId);

    await unitOfWork.CommitAsync();
  }
}
