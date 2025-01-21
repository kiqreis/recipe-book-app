using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Delete;

public class DeleteRecipe(ILoggedUser _loggedUser, IRecipeRepository repository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IDeleteRecipe
{
  public async Task Execute(long recipeId)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await repository.GetById(loggedUser, recipeId);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    if (!string.IsNullOrWhiteSpace(recipe.ImageId))
    {
      await blobStorageService.Delete(loggedUser, recipe.ImageId);
    }

    await repository.Delete(recipeId);

    await unitOfWork.CommitAsync();
  }
}
