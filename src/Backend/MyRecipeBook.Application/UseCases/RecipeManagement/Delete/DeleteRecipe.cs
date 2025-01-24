using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Delete;

public class DeleteRecipe(ILoggedUser _loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository, IRecipeWriteOnlyRepository recipeWriteOnlyRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IDeleteRecipe
{
  public async Task Execute(long recipeId)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await recipeReadOnlyRepository.GetById(loggedUser, recipeId);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    if (recipe.ImageId.NotEmpty())
    {
      await blobStorageService.Delete(loggedUser, recipe.ImageId);
    }

    await recipeWriteOnlyRepository.Delete(recipeId);

    await unitOfWork.CommitAsync();
  }
}
