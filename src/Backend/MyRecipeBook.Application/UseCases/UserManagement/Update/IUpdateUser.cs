using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.UserManagement.Update;

public interface IUpdateUser
{
  Task Execute(UpdateUserRequest request);
}
