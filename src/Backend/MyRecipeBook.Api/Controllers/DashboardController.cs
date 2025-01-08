using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Attributes;
using MyRecipeBook.Application.UseCases.RecipeManagement.Dashboard;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

[AuthenticateUser]
public class DashboardController : MyRecipeBookControllerBase
{
  [HttpGet]
  [ProducesResponseType<RecipesResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Get([FromServices] IGetDashboard getDashboard)
  {
    var response = await getDashboard.Execute();

    if (response.Recipes.Any())
    {
      return Ok(response);
    }

    return NoContent();
  }
}
