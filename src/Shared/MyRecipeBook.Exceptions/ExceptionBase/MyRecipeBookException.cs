using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionBase;

public abstract class MyRecipeBookException(string message) : SystemException(message)
{
  public abstract IList<string> GetErrorMessages();
  public abstract HttpStatusCode GetStatusCode();
}