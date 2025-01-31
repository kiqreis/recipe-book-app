namespace MyRecipeBook.Application.UseCases.UserManagement.ExternalLogin;

public interface IExternalLogin
{
  Task<string> Execute(string name, string email);
}
