using CommonTestsUtilities.Token;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.UserManagement.Profile;

public class GetUserProfileTest : MyRecipeBookClassFixture
{
  private readonly string method = "user";

  private readonly string _name;
  private readonly string _email;
  private readonly Guid _userId;

  public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _name = factory.GetName();
    _email = factory.GetEmail();
    _userId = factory.GetUserId();
  }

  [Fact]
  public async Task Success()
  {
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Get(method, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);

    responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
    responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email);
  }
}
