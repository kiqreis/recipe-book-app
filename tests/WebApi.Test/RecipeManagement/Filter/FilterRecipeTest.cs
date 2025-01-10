using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.RecipeManagement.Filter;

public class FilterRecipeTest : MyRecipeBookClassFixture
{
  private readonly string method = "recipe/filter";

  private readonly Guid _userId;

  private string _recipeTitle;
  private MyRecipeBook.Domain.Enums.Difficulty _recipeDifficulty;
  private MyRecipeBook.Domain.Enums.CookingTime _recipeCookingTime;
  private IList<MyRecipeBook.Domain.Enums.DishType> _recipeDishTypes;

  public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _userId = factory.GetUserId();

    _recipeTitle = factory.GetRecipeTitle();
    _recipeCookingTime = factory.GetRecipeCookingTime();
    _recipeDifficulty = factory.GetRecipeDifficulty();
    _recipeDishTypes = factory.GetDishTypes();
  }

  [Fact]
  public async Task Succes()
  {
    var request = new RecipeFilterRequest
    {
      CookingTimes = [(MyRecipeBook.Communication.Enums.CookingTime)_recipeCookingTime],
      Difficulties = [(MyRecipeBook.Communication.Enums.Difficulty)_recipeDifficulty],
      DishTypes = _recipeDishTypes.Select(d => (MyRecipeBook.Communication.Enums.DishType)d).ToList(),
      RecipeTitleIngredient = _recipeTitle
    };

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Post(method: method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
  }

  [Fact]
  public async Task Success_NoContent()
  {
    var request = RecipeFilterRequestBuilder.Build();
    request.RecipeTitleIngredient = "recipeDontExist";

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

    var response = await Post(method: method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_CookingTime_Invalid(string culture)
  {
    var request = RecipeFilterRequestBuilder.Build();
    request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)100);

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

    var response = await Post(method: method, request: request, token: token, culture: culture);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));

    errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
