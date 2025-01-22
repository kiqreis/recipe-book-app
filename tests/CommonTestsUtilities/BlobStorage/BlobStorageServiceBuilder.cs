using Bogus;
using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace CommonTestsUtilities.BlobStorage;

public class BlobStorageServiceBuilder
{
  private readonly Mock<IBlobStorageService> _mock;

  public BlobStorageServiceBuilder() => _mock = new Mock<IBlobStorageService>();

  public BlobStorageServiceBuilder GetImageUrl(User user, string? fileName)
  {
    if (string.IsNullOrWhiteSpace(fileName))
    {
      return this;
    }

    var faker = new Faker();
    var imageUrl = faker.Image.LoremFlickrUrl();

    _mock.Setup(blobStorage => blobStorage.GetImageUrl(user, fileName)).ReturnsAsync(imageUrl);

    return this;
  }

  public BlobStorageServiceBuilder GetImageUrl(User user, IList<Recipe> recipes)
  {
    foreach (var recipe in recipes)
    {
      GetImageUrl(user, recipe.ImageId);
    }

    return this;
  }

  public IBlobStorageService Build() => _mock.Object;
}
