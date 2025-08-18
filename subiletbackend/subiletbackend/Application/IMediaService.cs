using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SubiletBackend.Application
{
    public interface IMediaService
    {
        Task<string> UploadImageAsync(IFormFile file, string containerName);
        Task DeleteImageAsync(string blobName, string containerName);
    }
} 