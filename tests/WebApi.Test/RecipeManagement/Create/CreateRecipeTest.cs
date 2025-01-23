using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.RecipeManagement.Create;

public class CreateRecipeTest : MyRecipeBookClassFixture
{
  private readonly string method = "recipe";
  private readonly Guid _userId;

  public CreateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _userId = factory.GetUserId();
  }

  [Fact]
  public async Task Success()
  {
    var request = CreateRecipeFormDataRequestBuilder.Build();
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await PostFormData(method: method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.Created);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Empty_Title(string culture)
  {
    var request = CreateRecipeFormDataRequestBuilder.Build();

    request.Title = string.Empty;

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await PostFormData(method: method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

    errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
