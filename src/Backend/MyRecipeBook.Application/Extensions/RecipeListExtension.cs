using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.Extensions;

public static class RecipeListExtension
{
  public static async Task<IList<RecipeResponseShort>> MapToShortRecipe(this IList<Recipe> recipes, User user, IBlobStorageService blobStorageService, IMapper mapper)
  {
    var result = recipes.Select(async recipe =>
    {
      var response = mapper.Map<RecipeResponseShort>(recipe);

      if (!string.IsNullOrWhiteSpace(recipe.ImageId))
      {
        response.ImageUrl = await blobStorageService.GetImageUrl(user, recipe.ImageId);
      }

      return response;
    });

    var response = await Task.WhenAll(result);

    return response;
  }
}
