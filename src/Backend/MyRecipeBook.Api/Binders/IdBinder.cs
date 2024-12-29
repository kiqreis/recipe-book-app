using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace MyRecipeBook.Api.Binders;

public class IdBinder(SqidsEncoder<long> encoder) : IModelBinder
{
  public Task BindModelAsync(ModelBindingContext bindingContext)
  {
    var modelName = bindingContext.ModelName;
    var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

    if (valueProviderResult == ValueProviderResult.None)
    {
      return Task.CompletedTask;
    }

    bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

    var value = valueProviderResult.FirstValue;

    if (string.IsNullOrEmpty(value))
    {
      return Task.CompletedTask;
    }

    var id = encoder.Decode(value).Single();

    bindingContext.Result = ModelBindingResult.Success(id);

    return Task.CompletedTask;
  }
}
