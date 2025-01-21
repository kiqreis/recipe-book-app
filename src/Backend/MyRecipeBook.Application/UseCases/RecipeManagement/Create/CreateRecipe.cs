using AutoMapper;
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.RecipeRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.RecipeManagement.Create;

public class CreateRecipe(IRecipeRepository repository, ILoggedUser _loggedUser, IUnitOfWork unitOfWork, IMapper mapper, IBlobStorageService blobStorageService) : ICreateRecipe
{
  public async Task<CreatedRecipeResponse> Execute(CreateRecipeRequestFormData request)
  {
    Validate(request);

    var loggedUser = await _loggedUser.User();
    var recipe = mapper.Map<Recipe>(request);

    recipe.UserId = loggedUser.Id;

    var instructions = request.Instructions.OrderBy(instruction => instruction.Step).ToList();

    for (var i = 0; i < instructions.Count; i++)
    {
      instructions[i].Step = i + 1;
    }

    recipe.Instructions = mapper.Map<IList<Instruction>>(instructions);

    if (request.Image != null)
    {
      recipe.ImageId = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

      var fileStream = request.Image.OpenReadStream();
      
      if (!fileStream.Is<PortableNetworkGraphic>() && !fileStream.Is<JointPhotographicExpertsGroup>())
      {
        throw new RequestValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
      }

      fileStream.Position = 0;

      await blobStorageService.Upload(loggedUser, fileStream, recipe.ImageId);
    }

    await repository.Add(recipe);
    await unitOfWork.CommitAsync();

    return mapper.Map<CreatedRecipeResponse>(recipe);
  }

  public static void Validate(RecipeRequest request)
  {
    var result = new RecipeValidator().Validate(request);

    if (result.IsValid == false)
    {
      throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
  }
}
