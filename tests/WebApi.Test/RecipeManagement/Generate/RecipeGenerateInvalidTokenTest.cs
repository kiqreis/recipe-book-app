using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.RecipeManagement.Generate;

public class RecipeGenerateInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
  private readonly string method = "recipe/generate";

  [Fact]
  public async Task Error_Invalid_Tokne()
  {
    var request = RecipeGenerateRequestBuilder.Build();
    var response = await Post(method: method, request: request, token: "invalidToken");

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Without_Token()
  {
    var request = RecipeGenerateRequestBuilder.Build();
    var response = await Post(method: method, request: request, token: string.Empty);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Token_With_User_Not_Found()
  {
    var request = RecipeGenerateRequestBuilder.Build();
    var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
    var response = await Post(method: method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }
}
