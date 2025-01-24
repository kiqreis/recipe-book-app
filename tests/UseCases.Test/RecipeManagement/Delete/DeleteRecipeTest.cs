using CommonTestsUtilities.BlobStorage;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Delete;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.RecipeManagement.Delete;

public class DeleteRecipeTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);
    var useCase = DeleteRecipe(user, recipe);

    Func<Task> action = async () => await useCase.Execute(recipe.Id);

    await action.Should().NotThrowAsync();
  }

  [Fact]
  public async Task Error_Recipe_Not_Found()
  {
    (var user, _) = UserBuilder.Build();
    var useCase = DeleteRecipe(user);

    Func<Task> action = async () => await useCase.Execute(recipeId: 1000);

    (await action.Should().ThrowAsync<NotFoundException>())
      .Where(e => e.Message.Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
  }

  public static DeleteRecipe DeleteRecipe(User user, Recipe? recipe = null)
  {
    var loggedUser = LoggedUserBuilder.Build(user);
    var recipeReadOnlyRepository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
    var recipeWriteOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
    var unitOfWork = UnityOfWorkBuilder.Build();
    var blobStorage = new BlobStorageServiceBuilder().GetImageUrl(user, recipe?.ImageId).Build();

    return new DeleteRecipe(loggedUser, recipeReadOnlyRepository, recipeWriteOnlyRepository, unitOfWork, blobStorage);
  }
}
