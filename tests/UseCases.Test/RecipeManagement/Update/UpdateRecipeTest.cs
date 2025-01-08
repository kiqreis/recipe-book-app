using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Update;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.RecipeManagement.Update;

public class UpdateRecipeTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);
    var request = RecipeRequestBuilder.Build();
    var useCase = UpdateRecipe(user, recipe);

    Func<Task> action = async () => await useCase.Execute(recipe.Id, request);

    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task Error_Recipe_Not_Found()
  {
    (var user, _) = UserBuilder.Build();
    var request = RecipeRequestBuilder.Build();
    var useCase = UpdateRecipe(user);

    Func<Task> action = async () => await useCase.Execute(id: 1000, request);

    (await action.Should().ThrowAsync<NotFoundException>())
      .Where(e => e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
  }

  [Fact]
  public async Task Error_Empty_Title()
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);
    var request = RecipeRequestBuilder.Build();

    request.Title = string.Empty;

    var useCase = UpdateRecipe(user, recipe);

    Func<Task> action = async () => await useCase.Execute(recipe.Id, request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.RECIPE_TITLE_EMPTY));
  }

  private static UpdateRecipe UpdateRecipe(User user, Recipe? recipe = null)
  {
    var mapper = MapperBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var unitOfWork = UnityOfWorkBuilder.Build();
    var repository = new RecipeRepositoryBuilder().GetByIdUpdate(user, recipe).Build();

    return new UpdateRecipe(loggedUser, unitOfWork, mapper, repository);
  }
}
