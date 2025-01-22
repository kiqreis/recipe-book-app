using CommonTestsUtilities.BlobStorage;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.UseCases.RecipeManagement.Image;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using UseCases.Test.RecipeManagement.InlineData;

namespace UseCases.Test.RecipeManagement.Image;

public class AddUpdateImageTest
{
  [Theory]
  [ClassData(typeof(ImageTypesInlineData))]
  public async Task Success(IFormFile file)
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);
    var useCase = AddUpdateImage(user, recipe);
    var action = async () => await useCase.Execute(recipe.Id, file);

    await action.Should().NotThrowAsync();
  }

  [Theory]
  [ClassData(typeof(ImageTypesInlineData))]
  public async Task Success_Recipe_Did_Not_Have_Image(IFormFile file)
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);

    recipe.ImageId = null;

    var useCase = AddUpdateImage(user, recipe);
    var action = async () => await useCase.Execute(recipe.Id, file);

    await action.Should().NotThrowAsync();

    recipe.ImageId.Should().NotBeNullOrWhiteSpace();
  }

  [Theory]
  [ClassData(typeof(ImageTypesInlineData))]
  public async Task Error_Recipe_NotFound(IFormFile file)
  {
    (var user, _) = UserBuilder.Build();
    var useCase = AddUpdateImage(user);
    var action = async () => await useCase.Execute(1, file);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Equals(ResourceMessagesException.RECIPE_NOT_FOUND));
  }

  [Fact]
  public async Task Error_When_File_Is_Text()
  {
    (var user, _) = UserBuilder.Build();
    var recipe = RecipeBuilder.Build(user);
    var useCase = AddUpdateImage(user, recipe);
    var file = FormFileBuilder.Txt();
    var action = async () => await useCase.Execute(recipe.Id, file);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
  }

  private static AddUpdateImage AddUpdateImage(User user, Recipe? recipe = null)
  {
    var loggedUser = LoggedUserBuilder.Build(user);
    var repository = new RecipeRepositoryBuilder().GetById(user, recipe).Build();
    var unitOfWork = UnityOfWorkBuilder.Build();
    var blobStorage = new BlobStorageServiceBuilder().Build();

    return new AddUpdateImage(loggedUser, repository, unitOfWork, blobStorage);
  }
}
