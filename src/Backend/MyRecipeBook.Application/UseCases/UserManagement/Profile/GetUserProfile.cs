using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.UserManagement.Profile;

public class GetUserProfile(ILoggedUser loggedUser, IMapper mapper) : IGetUserProfile
{
  public async Task<UserProfileResponse> Execute()
  {
    var user = await loggedUser.User();

    return mapper.Map<UserProfileResponse>(user);
  }
}
