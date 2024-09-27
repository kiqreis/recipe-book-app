namespace MyRecipeBook.Exceptions.ExceptionBase;

public class RequestValidationException(IList<string> errors) : MyRecipeBookException
{
  public IList<string> ErrorMessages { get; set; } = errors;
}