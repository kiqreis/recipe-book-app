using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Security.Token;

namespace MyRecipeBook.Application.UseCases.UserManagement.ExternalLogin;

public class ExternalLogin(IUserReadOnlyRepository userReadOnlyRepository, IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator) : IExternalLogin
{
  public async Task<string> Execute(string name, string email)
  {
    var user = await userReadOnlyRepository.GetByEmail(email);

    if (user == null)
    {
      user = new Domain.Entities.User
      {
        Name = name,
        Email = email,
        UserId = Guid.NewGuid(),
        Password = "-"
      };
    }

    await userWriteOnlyRepository.Add(user);
    await unitOfWork.CommitAsync();

    return accessTokenGenerator.Generate(user.UserId);
  }
}
