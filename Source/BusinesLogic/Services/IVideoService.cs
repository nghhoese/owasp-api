using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace BusinesLogic.Services;

public interface IVideoService
{
    Task<IEnumerable<Video>> GetVideos();

    Task<Video?> GetVideo(int id);

    Task<Video?> AddVideo(string title, string description, IFormFile file);
}