using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Encryption;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;

public class ChangePassword(ILoggedUser _loggedUser, IPasswordEncrypt passwordEncrypt, IUserUpdateOnlyRepository userUpdateOnlyRepository, IUnitOfWork unitOfWork) : IChangePassword
{
  public async Task Execute(ChangePasswordRequest request)
  {
    var loggedUser = await _loggedUser.User();

    Validate(request, loggedUser);

    var user = await userUpdateOnlyRepository.GetById(loggedUser.Id);

    user.Password = passwordEncrypt.Encrypt(request.NewPassword);

    userUpdateOnlyRepository.Update(user);

    await unitOfWork.CommitAsync();
  }

  public void Validate(ChangePasswordRequest request, User user)
  {
    var result = new ChangePasswordValidator().Validate(request);

    if (passwordEncrypt.IsValid(request.Password, user.Password).IsFalse())
    {
      result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.DIFFERENT_PASSWORD_FROM_THE_CURRENT));
    }

    if (result.IsValid.IsFalse())
    {
      throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
  }
}
