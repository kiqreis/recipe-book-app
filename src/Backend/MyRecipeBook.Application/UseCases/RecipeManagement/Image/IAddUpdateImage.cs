using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Image;

public interface IAddUpdateImage
{
  Task Execute(long id, IFormFile file);
}
