namespace MyRecipeBook.Exceptions.ExceptionBase;

public class RequestValidationException(IList<string> errors) : MyRecipeBookException(string.Empty)
{
  public IList<string> ErrorMessages { get; set; } = errors;
}