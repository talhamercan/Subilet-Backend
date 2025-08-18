using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using SubiletBackend.Application;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SubiletBackend.Infrastructure
{
    public class AzureBlobMediaService : IMediaService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobMediaService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();
            var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var blobClient = container.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            return blobClient.Uri.ToString();
        }

        public async Task DeleteImageAsync(string blobName, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
} 