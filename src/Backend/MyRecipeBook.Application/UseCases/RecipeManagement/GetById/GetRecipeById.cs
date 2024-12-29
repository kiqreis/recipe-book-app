using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.GetById;

public class GetRecipeById(IMapper mapper, ILoggedUser _loggedUser, IRecipeRepository repository) : IGetRecipeById
{
  public async Task<RecipeResponse> Execute(long id)
  {
    var loggedUser = await _loggedUser.User();
    var recipe = await repository.GetById(loggedUser, id);

    if (recipe == null)
    {
      throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
    }

    return mapper.Map<RecipeResponse>(recipe);
  }
}
