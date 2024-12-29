using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public class RequestValidationException(IList<string> errors) : MyRecipeBookException(string.Empty)
{
  private readonly IList<string> ErrorMessages = errors;

  public override IList<string> GetErrorMessages() => ErrorMessages;

  public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}