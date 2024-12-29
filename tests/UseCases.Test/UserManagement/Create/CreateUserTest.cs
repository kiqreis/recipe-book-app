using CommonTestsUtilities.Encrypt;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

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
    response.Token.Should().NotBeNull();
    response.Name.Should().Be(request.Name);
    response.Email.Should().Be(request.Email);
    response.Token.AccessToken.Should().NotBeNullOrEmpty();
  }

  [Fact]
  public async Task Error_Email_Already_Exists()
  {
    var request = CreateUserRequestBuilder.Build();
    var useCase = CreateUser(request.Email);

    Func<Task> action = async () => await useCase.Execute(request);

    (await action.Should().ThrowAsync<RequestValidationException>())
      .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_EXISTS));
  }

  private CreateUser CreateUser(string? email = null)
  {
    var mapper = MapperBuilder.Build();
    var passwordEncrypt = PasswordEncryptBuilder.Build();
    var repository = new UserRepositoryBuilder();
    var unitOfWork = UnityOfWorkBuilder.Build();
    var accessToken = JwtTokenGeneratorBuilder.Build();

    if (string.IsNullOrEmpty(email) == false)
    {
      repository.IsActiveUserWithEmail(email);
    }

    return new CreateUser(mapper, passwordEncrypt, repository.Build(), unitOfWork, accessToken);
  }
}
