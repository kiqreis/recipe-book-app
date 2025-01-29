using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.RecipeManagement.Filter;

public class FilterRecipeInvalidToken : MyRecipeBookClassFixture
{
  private readonly string _method = "recipe/filter";

  public FilterRecipeInvalidToken(CustomWebApplicationFactory factory) : base(factory)
  {
  }

  [Fact]
  public async Task Error_Token_Invalid()
  {
    var request = RecipeFilterRequestBuilder.Build();
    var response = await Post(method: _method, request: request, token: "invalidToken");

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Without_Token()
  {
    var request = RecipeFilterRequestBuilder.Build();
    var response = await Post(method: _method, request: request, token: string.Empty);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async Task Error_Token_With_User_Not_Found()
  {
    var request = RecipeFilterRequestBuilder.Build();
    var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
    var response = await Post(method: _method, request: request, token: token);

    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }
}
