using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.UserManagement.ChangePassword;

public class ChangePasswordTest : MyRecipeBookClassFixture
{
  private readonly string method = "user/change-password";

  private readonly string _password;
  private readonly string _email;
  private readonly Guid _userId;

  public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _password = factory.GetPassword();
    _email = factory.GetEmail();
    _userId = factory.GetUserId();
  }

  [Fact]
  public async Task Success()
  {
    var request = ChangePasswordRequestBuilder.Build();

    request.Password = _password;

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Put(method, request, token);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);

    var loginRequest = new RequestLogin
    {
      Email = _email,
      Password = _password
    };

    response = await Post("login", loginRequest);
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    loginRequest.Password = request.NewPassword;

    response = await Post("login", loginRequest);

    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_New_Password_Empty(string culture)
  {
    var request = new ChangePasswordRequest
    {
      Password = _password,
      NewPassword = string.Empty
    };

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Put(method, request, token);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture));

    errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
  }
}
