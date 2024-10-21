using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test;

public class CreateUserTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
  private readonly string method = "user";
  private readonly HttpClient _httpClient = factory.CreateClient();

  [Fact]
  public async Task Success()
  {
    var request = CreateUserRequestBuilder.Build();
    var response = await _httpClient.PostAsJsonAsync(method, request);

    response.StatusCode.Should().Be(HttpStatusCode.Created);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(request.Name);

    responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(request.Email);
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Empty_Name(string culture)
  {
    var request = CreateUserRequestBuilder.Build();
    request.Name = string.Empty;

    if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
    {
      _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
    }

    _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

    var response = await _httpClient.PostAsJsonAsync(method, request);

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

    if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
    {
      _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
    }

    _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

    var response = await _httpClient.PostAsJsonAsync(method, request);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture));

    errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
