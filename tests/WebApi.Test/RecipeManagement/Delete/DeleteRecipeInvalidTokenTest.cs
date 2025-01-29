using CommonTestsUtilities.IdEncrypt;
using CommonTestsUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.RecipeManagement.Delete;

public class DeleteRecipeInvalidTokenTest : MyRecipeBookClassFixture
{
  private readonly string _method = "recipe";

  public DeleteRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
  {
  }

  [Fact]
  public async Task Error_Invalid_Token()
  {
    var id = IdEncryptBuilder.Build().Encode(1);
    var response = await Delete($"{_method}/{id}", token: "invalidToken");

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Token_With_User_Not_Found()
  {
    var id = IdEncryptBuilder.Build().Encode(1);
    var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
    var response = await Delete($"{_method}/{id}", token: token);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Without_Token()
  {
    var id = IdEncryptBuilder.Build().Encode(1);
    var response = await Delete($"{_method}/{id}", token: string.Empty);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }
}
