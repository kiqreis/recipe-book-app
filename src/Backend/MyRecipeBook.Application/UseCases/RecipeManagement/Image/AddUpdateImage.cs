using Microsoft.AspNetCore.Http;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Image;

public class AddUpdateImage(ILoggedUser _loggedUser, IRecipeRepository repository, IUnitOfWork unitOfWork) : IAddUpdateImage
{
  public async Task Execute(long id, IFormFile file)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await repository.GetById(loggedUser, id);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }
  }
}
