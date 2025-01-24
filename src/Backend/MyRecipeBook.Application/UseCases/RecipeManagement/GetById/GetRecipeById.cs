using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.GetById;

public class GetRecipeById(IMapper mapper, ILoggedUser _loggedUser, IRecipeReadOnlyRepository recipeReadOnlyRepository, IBlobStorageService blobStorageService) : IGetRecipeById
{
  public async Task<RecipeResponse> Execute(long id)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await recipeReadOnlyRepository.GetById(loggedUser, id);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    var response = mapper.Map<RecipeResponse>(recipe);

    if (!string.IsNullOrWhiteSpace(recipe.ImageId))
    {
      var url = await blobStorageService.GetImageUrl(loggedUser, recipe.ImageId);

      response.ImageUrl = url;
    }

    return response;
  }
}
