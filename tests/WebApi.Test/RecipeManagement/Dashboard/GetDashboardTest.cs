using CommonTestsUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.RecipeManagement.Dashboard;

public class GetDashboardTest : MyRecipeBookClassFixture
{
  private readonly string _method = "dashboard";

  private readonly Guid _userId;

  public GetDashboardTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _userId = factory.GetUserId();
  }

  [Fact]
  public async Task Success()
  {
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Get(method: _method, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("recipes").GetArrayLength().Should().BeGreaterThan(0);
  }
}
