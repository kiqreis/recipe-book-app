using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Filter;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace UseCases.Test.RecipeManagement.Filter;

public class FilterRecipeTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var request = RecipeFilterRequestBuilder.Build();
    var recipes = RecipeBuilder.Collection(user);
    var useCase = FilterRecipe(user, recipes);
    var result = await useCase.Execute(request);

    result.Should().NotBeNull();
    result.Recipes.Should().NotBeNullOrEmpty();
    result.Recipes.Should().HaveCount(recipes.Count);
  }

  [Fact]
  public async Task Error_Invalid_CookingTime()
  {
    (var user, _) = UserBuilder.Build();
    var recipes = RecipeBuilder.Collection(user);
    var request = RecipeFilterRequestBuilder.Build();

    request.CookingTimes.Add((CookingTime)100);

    var useCase = FilterRecipe(user, recipes);

    Func<Task> action = async () =>
    {
      await useCase.Execute(request);
    };

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
  }

  [Fact]
  public async Task Error_Invalid_Difficulty()
  {
    (var user, _) = UserBuilder.Build();
    var recipes = RecipeBuilder.Collection(user);
    var request = RecipeFilterRequestBuilder.Build();

    request.Difficulties.Add((Difficulty)100);

    var useCase = FilterRecipe(user, recipes);

    Func<Task> action = async () =>
    {
      await useCase.Execute(request);
    };

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
  }

  [Fact]
  public async Task Error_Invalid_DishType()
  {
    (var user, _) = UserBuilder.Build();
    var recipes = RecipeBuilder.Collection(user);
    var request = RecipeFilterRequestBuilder.Build();

    request.DishTypes.Add((MyRecipeBook.Communication.Enums.DishType)100);

    var useCase = FilterRecipe(user, recipes);

    Func<Task> action = async () =>
    {
      await useCase.Execute(request);
    };

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
  }

  private static FilterRecipe FilterRecipe(User user, IList<Recipe> recipes)
  {
    var mapper = MapperBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var repository = new RecipeRepositoryBuilder().Filter(user, recipes).Build();

    return new FilterRecipe(mapper, loggedUser, repository);
  }
}
