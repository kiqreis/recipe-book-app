using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.UserManagement.Update;

public interface IUpdateUser
{
  public Task Execute(UpdateUserRequest request);
}
