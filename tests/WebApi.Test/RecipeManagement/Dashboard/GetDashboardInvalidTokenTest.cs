using CommonTestsUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.RecipeManagement.Dashboard;

public class GetDashboardInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
  private readonly string method = "dashboard";

  [Fact]
  public async Task Error_Invalid_Token()
  {
    var response = await Get(method, token: "invalidToken");

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Without_Token()
  {
    var response = await Get(method, token: string.Empty);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Token_With_User_Not_Found()
  {
    var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
    var response = await Get(method, token);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }
}
