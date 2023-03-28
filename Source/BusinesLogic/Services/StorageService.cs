using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BusinesLogic.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;

        public StorageService(IConfiguration configuration)
        {
            _storageConnectionString = configuration["BlobConnectionString"];
            _storageContainerName = configuration["BlobContainerName"];
        }

        public async Task<Stream?> Download(string blobFilename)
        {
            BlobContainerClient client = new(_storageConnectionString, _storageContainerName);
            BlobClient file = client.GetBlobClient(blobFilename);
            // Check if the file exists in the container
            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;
                // Download the file details async
                //var content = await file.DownloadContentAsync();
                // Add data to variables in order to return a BlobDto
                //string name = blobFilename;
                //string contentType = content.Value.Details.ContentType;
                // Create new BlobDto with blob data from variables
                return blobContent;
            }
            return null;
        }

        public async Task<string> Upload(IFormFile blob)
        {
            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new(_storageConnectionString, _storageContainerName);
            //await container.CreateAsync();

            // Get a reference to the blob just uploaded from the API in a container from configuration settings
            BlobClient client = container.GetBlobClient(blob.Name + $"_{Path.GetRandomFileName() + Path.GetExtension(blob.FileName).ToLowerInvariant()}");
            // Open a stream for the file we want to upload
            await using (Stream? data = blob.OpenReadStream())
            {
                // Upload the file async
                await client.UploadAsync(data);
            }
            // Everything is OK and file got uploaded
            return client.Uri.AbsoluteUri;
        }
    }
}