using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Security.Token;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Create;

public class CreateUser(IUserWriteOnlyRepository writeOnlyRepository, IUserReadOnlyRepository readOnlyRepository, IMapper mapper, IPasswordEncrypt encrypt, IUnitOfWork unitOfWork, IAccessTokenGenerator accessToken) : ICreateUser
{
  public async Task<CreateUserResponse> Execute(CreateUserRequest request)
  {
    await Validate(request);

    var user = mapper.Map<User>(request);
    user.Password = encrypt.Encrypt(request.Password);
    user.UserId = Guid.NewGuid();

    await writeOnlyRepository.Add(user);
    await unitOfWork.CommitAsync();

    return new CreateUserResponse
    {
      Name = user.Name,
      Email = user.Email,
      Token = new TokenResponse
      {
        AccessToken = accessToken.Generate(user.UserId)
      }
    };
  }

  private async Task Validate(CreateUserRequest request)
  {
    var validator = new CreateUserValidator();
    var result = await validator.ValidateAsync(request);

    var emailExists = await readOnlyRepository.IsActiveUserWithEmail(request.Email);

    if (emailExists)
    {
      result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_EXISTS));
    }

    if (result.IsValid.IsFalse())
    {
      var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

      throw new RequestValidationException(errorMessages);
    }
  }
}