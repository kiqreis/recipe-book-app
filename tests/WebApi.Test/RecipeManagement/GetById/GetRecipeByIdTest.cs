using CommonTestsUtilities.IdEncrypt;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.RecipeManagement.GetById;

public class GetRecipeByIdTest : MyRecipeBookClassFixture
{
  private readonly string _method = "recipe";

  private readonly Guid _userId;
  private readonly string _recipeId;
  private readonly string _recipeTitle;

  public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _userId = factory.GetUserId();
    _recipeId = factory.GetRecipeId();
    _recipeTitle = factory.GetRecipeTitle();
  }

  [Fact]
  public async Task Success()
  {
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Get($"{_method}/{_recipeId}", token: token);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("id").GetString().Should().Be(_recipeId);
    responseData.RootElement.GetProperty("title").GetString().Should().Be(_recipeTitle);
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Recipe_Not_Found(string culture)
  {
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var id = IdEncryptBuilder.Build().Encode(1000);
    var response = await Get($"{_method}/{id}", token, culture);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

    errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
