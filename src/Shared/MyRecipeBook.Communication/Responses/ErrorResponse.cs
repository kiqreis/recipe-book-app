namespace MyRecipeBook.Communication.Responses;

public class ErrorResponse(IList<string> errors)
{
  public IList<string> Errors { get; set; } = errors;
  public bool TokenIsExpired { get; set; }

  public ErrorResponse(string error) : this([error])
  {
  }
}