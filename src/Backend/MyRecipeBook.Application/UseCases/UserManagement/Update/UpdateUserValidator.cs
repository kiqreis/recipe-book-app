using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.UserManagement.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
  public UpdateUserValidator()
  {
    RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
    RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);

    When(request => string.IsNullOrEmpty(request.Email) is false, () =>
    {
      RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
    });
  }
}
