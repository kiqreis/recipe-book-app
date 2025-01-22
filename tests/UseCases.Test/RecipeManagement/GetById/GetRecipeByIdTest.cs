using CommonTestsUtilities.BlobStorage;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.GetById;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.RecipeManagement.GetById;

public class GetRecipeByIdTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);
    var useCase = GetByIdRecipe(user, recipe);
    var result = await useCase.Execute(recipe.Id);

    result.Should().NotBeNull();
    result.Id.Should().NotBeNullOrWhiteSpace();
    result.Title.Should().Be(recipe.Title);
    result.ImageUrl.Should().NotBeNullOrWhiteSpace();
  }

  [Fact]
  public async Task Error_Recipe_Not_Found()
  {
    (var user, _) = UserBuilder.Build();
    var useCase = GetByIdRecipe(user);

    Func<Task> action = async () => await useCase.Execute(id: 1000);

    (await action.Should().ThrowAsync<NotFoundException>())
      .Where(e => e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
  }

  private static GetRecipeById GetByIdRecipe(User user, Recipe? recipe = null)
  {
    var mapper = MapperBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var repository = new RecipeRepositoryBuilder().GetById(user, recipe).Build();
    var blobStorage = new BlobStorageServiceBuilder().GetImageUrl(user, recipe?.ImageId).Build();

    return new GetRecipeById(mapper, loggedUser, repository, blobStorage);
  }
}
