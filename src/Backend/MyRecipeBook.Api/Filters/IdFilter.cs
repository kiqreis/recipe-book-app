using Microsoft.OpenApi.Models;
using MyRecipeBook.Api.Binders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyRecipeBook.Api.Filters;

public class IdFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var enrcyptedId = context.ApiDescription.ParameterDescriptions
      .Where(x => x.ModelMetadata.BinderType == typeof(IdBinder))
      .ToDictionary(d => d.Name, d => d);

    foreach (var param in operation.Parameters)
    {
      if (enrcyptedId.TryGetValue(param.Name, out var apiParam))
      {
        param.Schema.Format = string.Empty;
        param.Schema.Type = "string";
      }
    }

    foreach (var schema in context.SchemaRepository.Schemas.Values)
    {
      foreach (var prop in schema.Properties)
      {
        if (enrcyptedId.TryGetValue(prop.Key, out var apiParam))
        {
          prop.Value.Format = string.Empty;
          prop.Value.Type = "string";
        }
      }
    }
  }
}
