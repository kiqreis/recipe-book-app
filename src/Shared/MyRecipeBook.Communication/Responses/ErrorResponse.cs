namespace MyRecipeBook.Communication.Responses;

public class ErrorResponse(IList<string> errors)
{
  public IList<string> Errors { get; set; } = errors;

  public ErrorResponse(string error) : this([error])
  {
  }
}