using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.RecipeManagement.Update;

public class UpdateRecipeTest : MyRecipeBookClassFixture
{
  private readonly string _method = "recipe";

  private readonly Guid _userId;
  private readonly string _recipeId;

  public UpdateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _userId = factory.GetUserId();
    _recipeId = factory.GetRecipeId();
  }

  [Fact]
  public async Task Success()
  {
    var request = RecipeRequestBuilder.Build();
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Put($"{_method}/{_recipeId}", request, token);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Empty_Tilte(string culture)
  {
    var request = RecipeRequestBuilder.Build();

    request.Title = string.Empty;

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Put($"{_method}/{_recipeId}", request, token, culture);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

    errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
