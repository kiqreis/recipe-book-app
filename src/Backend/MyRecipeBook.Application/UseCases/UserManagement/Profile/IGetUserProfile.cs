using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.UserManagement.Profile;

public interface IGetUserProfile
{
  Task<UserProfileResponse> Execute();
}
