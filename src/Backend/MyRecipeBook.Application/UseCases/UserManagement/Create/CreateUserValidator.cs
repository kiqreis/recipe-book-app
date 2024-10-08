using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.UserManagement.Create;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
  public CreateUserValidator()
  {
    RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
    RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
    RuleFor(u => u.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessagesException.PASSWORD_EMPTY);

    When(u => string.IsNullOrEmpty(u.Email) == false, () =>
    {
      RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
    });
  }
}