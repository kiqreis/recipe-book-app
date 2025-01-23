namespace MyRecipeBook.Application.UseCases.UserManagement.Delete.DeleteAccount;

public interface IDeleteAccountUser
{
  Task Execute(Guid userId);
}
