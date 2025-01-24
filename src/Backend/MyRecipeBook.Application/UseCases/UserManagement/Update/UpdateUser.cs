using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Update;

public class UpdateUser(ILoggedUser _loggedUser, IUserUpdateOnlyRepository userUpdateOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork) : IUpdateUser
{
  public async Task Execute(UpdateUserRequest request)
  {
    var loggedUser = await _loggedUser.User();

    await Validate(request, loggedUser.Email);

    var user = await userUpdateOnlyRepository.GetById(loggedUser.Id);

    user.Name = request.Name;
    user.Email = request.Email;

    userUpdateOnlyRepository.Update(user);

    await unitOfWork.CommitAsync();
  }

  private async Task Validate(UpdateUserRequest request, string currentEmail)
  {
    var validator = new UpdateUserValidator();
    var result = await validator.ValidateAsync(request);

    if (currentEmail.Equals(request.Email).IsFalse())
    {
      var userExists = await userReadOnlyRepository.IsActiveUserWithEmail(request.Email);

      if (userExists)
      {
        result.Errors.Add(new ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_EXISTS));
      }
    }

    if (result.IsValid.IsFalse())
    {
      var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

      throw new RequestValidationException(errorMessages);
    }
  }
}
