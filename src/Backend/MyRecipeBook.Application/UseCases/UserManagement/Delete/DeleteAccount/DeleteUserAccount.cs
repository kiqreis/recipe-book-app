using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.UserRepository;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.UserManagement.Delete.DeleteAccount;

public class DeleteUserAccount(IUserDeleteOnlyRepository userDeleteOnlyRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IDeleteUserAccount
{
  public async Task Execute(Guid userId)
  {
    await blobStorageService.DeleteContainer(userId);

    await userDeleteOnlyRepository.DeleteAccount(userId);

    await unitOfWork.CommitAsync();
  }
}
