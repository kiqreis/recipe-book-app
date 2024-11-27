using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.UserManagement.Update;

public class UpdateUserTest : MyRecipeBookClassFixture
{
  private const string method = "user";
  private readonly Guid _userId;

  public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
  {
    _userId = factory.GetUserId();
  }

  [Fact]
  public async Task Success()
  {
    var request = UpdateUserRequestBuilder.Build();
    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Put(method, request, token);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Theory]
  [ClassData(typeof(CultureInlineData))]
  public async Task Error_Empty_Name(string culture)
  {
    var request = UpdateUserRequestBuilder.Build();

    request.Name = string.Empty;

    var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
    var response = await Put(method, request, token);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    await using var responseBody = await response.Content.ReadAsStreamAsync();

    var responseData = await JsonDocument.ParseAsync(responseBody);
    var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
    var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

    errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMessage));
  }
}
