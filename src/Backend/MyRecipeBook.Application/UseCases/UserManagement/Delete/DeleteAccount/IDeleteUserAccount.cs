namespace MyRecipeBook.Application.UseCases.UserManagement.Delete.DeleteAccount;

public interface IDeleteUserAccount
{
  Task Execute(Guid userId);
}
