using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.UserManagement.Create;

public interface ICreateUser
{
  public Task<CreateUserResponse> Execute(CreateUserRequest request);
}
