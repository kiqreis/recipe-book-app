using CommonTestsUtilities.BlobStorage;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.UseCases.RecipeManagement.Create;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using UseCases.Test.RecipeManagement.InlineData;

namespace UseCases.Test.RecipeManagement.Create;

public class CreateRecipeTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var request = CreateRecipeFormDataRequestBuilder.Build();
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
    var request = CreateRecipeFormDataRequestBuilder.Build();

    request.Title = string.Empty;

    var useCase = CreateRecipe(user);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.RECIPE_TITLE_EMPTY));
  }

  [Fact]
  public async Task Success_Without_Image()
  {
    (var user, _) = UserBuilder.Build();
    var request = CreateRecipeFormDataRequestBuilder.Build();
    var useCase = CreateRecipe(user);
    var result = await useCase.Execute(request);

    result.Should().NotBeNull();
    result.Id.Should().NotBeNullOrWhiteSpace();
  }

  [Theory]
  [ClassData(typeof(ImageTypesInlineData))]
  public async Task Success_With_Image(IFormFile file)
  {
    (var user, _) = UserBuilder.Build();
    var request = CreateRecipeFormDataRequestBuilder.Build(file);
    var useCase = CreateRecipe(user);
    var result = await useCase.Execute(request);

    result.Should().NotBeNull();
    result.Id.Should().NotBeNullOrWhiteSpace();
    result.Title.Should().Be(request.Title);
  }

  [Fact]
  public async Task Error_Invalid_File()
  {
    (var user, _) = UserBuilder.Build();
    var textFile = FormFileBuilder.Txt();
    var request = CreateRecipeFormDataRequestBuilder.Build(textFile);
    var useCase = CreateRecipe(user);
    var action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
  }

  private static CreateRecipe CreateRecipe(User user)
  {
    var mapper = MapperBuilder.Build();
    var unitOfWork = UnityOfWorkBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var recipeWriteOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
    var blobStorage = new BlobStorageServiceBuilder().Build();

    return new CreateRecipe(recipeWriteOnlyRepository, loggedUser, unitOfWork, mapper, blobStorage);
  }
}
