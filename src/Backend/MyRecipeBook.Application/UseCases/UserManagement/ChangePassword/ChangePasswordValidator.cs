using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.UserManagement.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
  public ChangePasswordValidator()
  {
    RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<ChangePasswordRequest>());
  }
}
