using CommonTestsUtilities.Entities;
using CommonTestsUtilities.LoggedUser;
using CommonTestsUtilities.Mapper;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Profile;
using MyRecipeBook.Domain.Entities;

namespace UseCases.Test.UserManagement.Profile;

public class GetUserProfileTest
{
  [Fact]
  public async Task Success()
  {
    (var user, var _) = UserBuilder.Build();

    var useCase = CreateUser(user);
    var result = await useCase.Execute();

    result.Should().NotBeNull();
    result.Name.Should().Be(user.Name);
    result.Email.Should().Be(user.Email);
  }

  private static GetUserProfile CreateUser(User user)
  {
    var mapper = MapperBuilder.Build();
    var loggedUser = LoggedUserBuilder.Build(user);

    return new GetUserProfile(loggedUser, mapper);
  }
}
