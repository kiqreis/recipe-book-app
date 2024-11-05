using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test;

public class LoginTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
  private readonly string _method = "login";
  private readonly HttpClient _httpClient = factory.CreateClient();
  private readonly string _email = factory.GetEmail();
  private readonly string _name = factory.GetName();
  private readonly string _password = factory.GetPassword();

  [Fact]
  public async Task Success()
  {
    var request = new RequestLogin
    {
      Email = _email,
      Password = _password
    };

    var response = await _httpClient.PostAsJsonAsync(_method, request);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(_name);
  }
}
