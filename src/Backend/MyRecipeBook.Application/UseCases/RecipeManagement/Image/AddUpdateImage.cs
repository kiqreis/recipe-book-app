using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
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

    if (!fileStream.Is<PortableNetworkGraphic>() && !fileStream.Is<JointPhotographicExpertsGroup>())
    {
      throw new RequestValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
    }

    if (string.IsNullOrWhiteSpace(recipe.ImageId))
    {
      recipe.ImageId = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

      repository.Update(recipe);

      await unitOfWork.CommitAsync();
    }

    fileStream.Position = 0;

    await blobStorageService.Upload(loggedUser, fileStream, recipe.ImageId);
  }
}
