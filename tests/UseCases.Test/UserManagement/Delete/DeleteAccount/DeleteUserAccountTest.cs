using CommonTestsUtilities.BlobStorage;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.UserManagement.Delete.DeleteAccount;

namespace UseCases.Test.UserManagement.Delete.DeleteAccount;

public class DeleteUserAccountTest
{
  [Fact]
  public async Task Success()
  {
    (var user, _) = UserBuilder.Build();
    var useCase = DeleteUserAccount();
    var action = async () => await useCase.Execute(user.UserId);

    await action.Should().NotThrowAsync();
  }

  private static DeleteUserAccount DeleteUserAccount()
  {
    var unitOfWork = UnitOfWorkBuilder.Build();
    var blobStorage = new BlobStorageServiceBuilder().Build();
    var repository = UserDeleteOnlyRepositoryBuilder.Build();

    return new DeleteUserAccount(repository, unitOfWork, blobStorage);
  }
}
