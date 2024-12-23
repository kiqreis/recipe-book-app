using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Create;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.RecipeManagement.Create;

public class CreateRecipeTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var request = RecipeRequestBuilder.Build();
    var useCase = CreateRecipe(user);
    var result = await useCase.Execute(request);

    result.Should().NotBeNull();
    result.Id.Should().NotBeNullOrWhiteSpace();
    result.Title.Should().Be(request.Title);
  }

  [Fact]
  public async Task Error_Empty_Title()
  {
    (var user, _) = UserBuilder.Build();
    var request = RecipeRequestBuilder.Build();

    request.Title = string.Empty;

    var useCase = CreateRecipe(user);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.RECIPE_TITLE_EMPTY));
  }

  private static CreateRecipe CreateRecipe(User user)
  {
    var mapper = MapperBuilder.Build();
    var unitOfWork = UnityOfWorkBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var repository = new RecipeRepositoryBuilder();

    return new CreateRecipe(repository.Build(), loggedUser, unitOfWork, mapper);
  }
}
