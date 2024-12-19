using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.RecipeManagement.Create;
using MyRecipeBook.Application.UseCases.RecipeManagement.Filter;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

public class RecipeController : MyRecipeBookControllerBase
{
  [HttpPost]
  [ProducesResponseType<CreatedRecipeResponse>(StatusCodes.Status201Created)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create([FromServices] ICreateRecipe createRecipe, RecipeRequest request)
  {
    var response = await createRecipe.Execute(request);

    return Created(string.Empty, response);
  }

  [HttpPost]
  [ProducesResponseType<RecipesResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Filter([FromServices] IFilterRecipe filterRecipe, RecipeFilterRequest request)
  {
    var response = await filterRecipe.Execute(request);

    if (response.Recipes.Any())
    {
      return Ok(response);
    }

    return NoContent();
  }
}
