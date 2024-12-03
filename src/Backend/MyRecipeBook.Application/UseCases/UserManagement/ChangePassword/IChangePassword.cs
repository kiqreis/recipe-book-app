using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;

public interface IChangePassword
{
    public Task Execute(ChangePasswordRequest request);
}
