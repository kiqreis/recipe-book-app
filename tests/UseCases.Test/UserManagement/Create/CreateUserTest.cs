using CommonTestsUtilities.Encrypt;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Create;
using MyRecipeBook.Domain.Extensions;
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
    response.Tokens.Should().NotBeNull();
    response.Name.Should().Be(request.Name);
    response.Email.Should().Be(request.Email);
    response.Tokens.AccessToken.Should().NotBeNullOrEmpty();
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
    var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
    var unitOfWork = UnitOfWorkBuilder.Build();
    var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
    var accessToken = JwtTokenGeneratorBuilder.Build();
    var tokenRepository = TokenRepositoryBuilder.Build();
    var refreshToken = new RefreshTokenGeneratorBuilder();

    if (email.NotEmpty())
    {
      userReadOnlyRepository.IsActiveUserWithEmail(email);
    }

    return new CreateUser(userWriteOnlyRepository, userReadOnlyRepository.Build(), mapper, passwordEncrypt, unitOfWork, accessToken, tokenRepository, refreshToken.Build());
  }
}
