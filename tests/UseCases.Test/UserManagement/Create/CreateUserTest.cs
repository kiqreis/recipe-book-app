using CommonTestsUtilities.Encrypt;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Create;

namespace UseCases.Test.UserManagement.Create;

public class CreateUserTest
{
  [Fact]
  public async Task Success()
  {
    var request = CreateUserRequestBuilder.Build();
    var useCase = CreateUser();

    var response = await useCase.Execute(request);

    response.Should().NotBeNull();
    response.Name.Should().Be(request.Name);
    response.Email.Should().Be(request.Email);
  }

  private CreateUser CreateUser()
  {
    var mapper = MapperBuilder.Build();
    var passwordEncrypt = PasswordEncryptBuilder.Build();
    var repository = UserRepositoryBuilder.Build();
    var unitOfWork = UnityOfWorkBuilder.Build();

    return new CreateUser(mapper, passwordEncrypt, repository, unitOfWork);
  }
}
