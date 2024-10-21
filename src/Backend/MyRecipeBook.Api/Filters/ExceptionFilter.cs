using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using System.Net;

namespace MyRecipeBook.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
  public void OnException(ExceptionContext context)
  {
    if (context.Exception is MyRecipeBookException)
    {
      HandleProjectException(context);
    }
    else
    {
      ThrowUnknownException(context);
    }
  }

  private void HandleProjectException(ExceptionContext context)
  {
    if (context.Exception is RequestValidationException)
    {
      var exception = context.Exception as RequestValidationException;
      
      context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Result = new BadRequestObjectResult(new ErrorResponse(exception!.ErrorMessages));
    }
  }

  private void ThrowUnknownException(ExceptionContext context)
  {
    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    context.Result = new ObjectResult(new ErrorResponse(ResourceMessagesException.UNKNOWN_ERROR));
  }
}