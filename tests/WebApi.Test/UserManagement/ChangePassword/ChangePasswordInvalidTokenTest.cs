using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using System.Net;

namespace WebApi.Test.UserManagement.ChangePassword;

public class ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
  private readonly string method = "user/change-password";

  [Fact]
  public async Task Error_Token_Invalid()
  {
    var request = new ChangePasswordRequest();
    var response = await Put(method: method, request: request, token: "invalidToken");

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Without_Token()
  {
    var request = new ChangePasswordRequest();
    var response = await Put(method: method, request: request, token: string.Empty);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Token_With_User_Not_Found()
  {
    var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
    var request = new ChangePasswordRequest();
    var response = await Put(method: method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }
}
