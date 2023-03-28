using Microsoft.AspNetCore.Http;

namespace BusinesLogic.Services
{
    public interface IStorageService
    {
        Task<string> Upload(IFormFile file);

        Task<Stream?> Download(string blobFilename);
    }
}