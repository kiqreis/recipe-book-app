using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Binders;
using MyRecipeBook.Application.UseCases.RecipeManagement.Create;
using MyRecipeBook.Application.UseCases.RecipeManagement.Filter;
using MyRecipeBook.Application.UseCases.RecipeManagement.GetById;
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

  [HttpPost("filter")]
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

  [HttpGet]
  [Route("{id}")]
  [ProducesResponseType<RecipeResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(
    [FromServices] IGetRecipeById recipeById,
    [FromRoute][ModelBinder(typeof(IdBinder))] long id)
  {
    var response = await recipeById.Execute(id);

    return Ok(response);
  }
}
