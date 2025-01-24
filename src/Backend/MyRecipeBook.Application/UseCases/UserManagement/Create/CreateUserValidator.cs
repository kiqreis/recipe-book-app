using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.UserManagement.Create;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
  public CreateUserValidator()
  {
    RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
    RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
    RuleFor(u => u.Password).SetValidator(new PasswordValidator<CreateUserRequest>());

    When(u => u.Email.NotEmpty(), () =>
    {
      RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
    });
  }
}