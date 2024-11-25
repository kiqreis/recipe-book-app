using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.UserManagement.Update;

public class UpdateUser(ILoggedUser _loggedUser, IUserRepository repository, IUnitOfWork unitOfWork) : IUpdateUser
{
  public async Task Execute(UpdateUserRequest request)
  {
    var loggedUser = await _loggedUser.User();

    await Validate(request, loggedUser.Email);

    var user = await repository.GetById(loggedUser.Id);

    user.Name = request.Name;
    user.Email = request.Email;

    repository.Update(user);

    await unitOfWork.CommitAsync();
  }

  private async Task Validate(UpdateUserRequest request, string currentEmail)
  {
    var validator = new UpdateUserValidator();
    var result = validator.Validate(request);

    if (currentEmail.Equals(request.Email) is false)
    {
      var userExist = await repository.IsActiveUserWithEmail(request.Email);

      if (userExist)
      {
        result.Errors.Add(new ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_EXISTS));
      }

      if (result.IsValid is false)
      {
        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

        throw new RequestValidationException(errorMessages);
      }
    }
  }
}
