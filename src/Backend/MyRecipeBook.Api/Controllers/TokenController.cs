using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.TokenManagement.RefreshToken;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

public class TokenController : MyRecipeBookControllerBase
{
  [HttpPost("refresh-token")]
  [ProducesResponseType<TokensResponse>(StatusCodes.Status200OK)]
  public async Task<IActionResult> RefreshToken([FromServices] IUseRefreshToken refreshToken, NewTokenRequest request)
  {
    var response = await refreshToken.Execute(request);

    return Ok(response);
  }
}
