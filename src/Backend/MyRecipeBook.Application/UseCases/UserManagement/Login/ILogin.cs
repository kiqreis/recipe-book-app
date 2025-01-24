using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.UserManagement.Login;

public interface ILogin
{
  Task<CreateUserResponse> Execute(RequestLogin request);
}
