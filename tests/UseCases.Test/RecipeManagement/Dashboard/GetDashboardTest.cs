﻿using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.RecipeManagement.Dashboard;
using MyRecipeBook.Domain.Entities;

namespace UseCases.Test.RecipeManagement.Dashboard;

public class GetDashboardTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var recipes = RecipeBuilder.Collection(user);
    var useCase = GetForDashboard(user, recipes);
    var result = await useCase.Execute();

    result.Should().NotBeNull();

    result.Recipes.Should()
      .HaveCountGreaterThan(0)
      .And.OnlyHaveUniqueItems(recipe => recipe.Id)
      .And.AllSatisfy(recipe =>
      {
        recipe.Id.Should().NotBeNullOrWhiteSpace();
        recipe.Title.Should().NotBeNullOrWhiteSpace();
        recipe.AmountIngredients.Should().BeGreaterThan(0);
      });
  }

  private static GetDashboard GetForDashboard(User user, IList<Recipe> recipes)
  {
    var mapper = MapperBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);
    var repository = new RecipeRepositoryBuilder().GetForDashboard(user, recipes).Build();

    return new GetDashboard(repository, mapper, loggedUser);
  }
}