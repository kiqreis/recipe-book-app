using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;

public class ChangePassword(ILoggedUser _loggedUser, IPasswordEncrypt passwordEncrypt, IUserRepository repository, IUnitOfWork unitOfWork) : IChangePassword
{
  public async Task Execute(ChangePasswordRequest request)
  {
    var loggedUser = await _loggedUser.User();

    Validate(request, loggedUser);

    var user = await repository.GetById(loggedUser.Id);

    user.Password = passwordEncrypt.Encrypt(request.NewPassword);

    repository.Update(user);

    await unitOfWork.CommitAsync();
  }

  public void Validate(ChangePasswordRequest request, User user)
  {
    var result = new ChangePasswordValidator().Validate(request);
    var currentPasswod = passwordEncrypt.Encrypt(request.Password);

    if (currentPasswod.Equals(user.Password) == false)
    {
      result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.DIFFERENT_PASSWORD_FROM_THE_CURRENT));
    }

    if (result.IsValid == false)
    {
      throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
  }
}
