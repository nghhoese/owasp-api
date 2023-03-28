using DataLayer.Data;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinesLogic.Services;

public class VideoService : IVideoService
{
    private readonly DataContext _dataContext;
    private readonly IStorageService _storageService;

    public VideoService(DataContext dataContext, IStorageService storageService)
    {
        _dataContext = dataContext;
        _storageService = storageService;
    }

    public async Task<IEnumerable<Video>> GetVideos()
    {
        return await _dataContext.Videos.ToListAsync();
    }

    public async Task<Video?> GetVideo(int id)
    {
        var video = await _dataContext.Videos.SingleOrDefaultAsync(video => video.Id == id);
        return video;
    }

    public async Task<Video?> AddVideo(string title, string description, IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        var filePath = await _storageService.Upload(file);
        Video video = new()
        {
            Title = title,
            Description = description,
            FilePath = filePath
        };
        var newVideo = await _dataContext.Videos.AddAsync(video);
        await _dataContext.SaveChangesAsync();
        return newVideo.Entity;
    }
}