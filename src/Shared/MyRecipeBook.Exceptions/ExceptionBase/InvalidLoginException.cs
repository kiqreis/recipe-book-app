namespace MyRecipeBook.Exceptions.ExceptionBase;

public class InvalidLoginException() : MyRecipeBookException(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
{
}
