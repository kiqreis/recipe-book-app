using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Domain.ValueObjects;

namespace MyRecipeBook.Infrastructure.Services.Storage;

public class AzureStorageService(BlobServiceClient blobServiceClient) : IBlobStorageService
{
  public async Task Upload(User user, Stream file, string fileName)
  {
    var container = blobServiceClient.GetBlobContainerClient(user.UserId.ToString());

    await container.CreateIfNotExistsAsync();

    var blobClient = container.GetBlobClient(fileName);

    await blobClient.UploadAsync(file, overwrite: true);
  }

  public async Task<string> GetImageUrl(User user, string fileName)
  {
    var containerName = user.UserId.ToString();
    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    var exist = await containerClient.ExistsAsync();

    if (exist.Value == false)
    {
      return string.Empty;
    }

    var blobClient = containerClient.GetBlobClient(fileName);

    exist = await blobClient.ExistsAsync();

    if (exist.Value)
    {
      var sasBuilder = new BlobSasBuilder
      {
        BlobContainerName = containerName,
        BlobName = fileName,
        Resource = "b",
        ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(MyRecipeBookRuleConstants.MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES)
      };

      sasBuilder.SetPermissions(BlobSasPermissions.Read);

      return blobClient.GenerateSasUri(sasBuilder).ToString();
    }

    return string.Empty;
  }
}
