using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Image;

public class AddUpdateImage(ILoggedUser _loggedUser, IRecipeRepository repository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IAddUpdateImage
{
  public async Task Execute(long id, IFormFile file)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await repository.GetById(loggedUser, id);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    var fileStream = file.OpenReadStream();

    (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

    if (isValidImage == false)
    {
      throw new RequestValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
    }

    if (string.IsNullOrWhiteSpace(recipe.ImageId))
    {
      recipe.ImageId = $"{Guid.NewGuid()}{extension}";

      repository.Update(recipe);

      await unitOfWork.CommitAsync();
    }

    await blobStorageService.Upload(loggedUser, fileStream, recipe.ImageId);
  }
}
