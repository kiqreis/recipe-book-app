using CommonTestsUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test;

public class CreateUserTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _httpClient = factory.CreateClient();

  [Fact]
  public async Task Success()
  {
    var request = CreateUserRequestBuilder.Build();
    var response = await _httpClient.PostAsJsonAsync("User", request);

    response.StatusCode.Should().Be(HttpStatusCode.Created);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(request.Name);

    responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace()
      .And.Be(request.Email);
  }
}
