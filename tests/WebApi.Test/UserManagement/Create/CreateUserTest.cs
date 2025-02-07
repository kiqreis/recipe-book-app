using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.UserManagement.Create;

public class CreateUserTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
  private readonly string method = "user";

  [Fact]
  public async Task Success()
  {
    var request = CreateUserRequestBuilder.Build();
    var response = await Post(method: method, request: request);

    response.StatusCode.Should().Be(HttpStatusCode.Created);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(request.Name);

    responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(request.Email);
    responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
    responseData.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString().Should().NotBeNullOrEmpty();
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Empty_Name(string culture)
  {
    var request = CreateUserRequestBuilder.Build();
    request.Name = string.Empty;

    var response = await Post(method: method, request: request, culture: culture);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

    errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Invalid_Email(string culture)
  {
    var request = CreateUserRequestBuilder.Build();
    request.Email = string.Empty;

    var response = await Post(method: method, request: request, culture: culture);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture));

    errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
