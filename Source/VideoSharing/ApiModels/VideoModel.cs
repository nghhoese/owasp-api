using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace VideoSharing.ApiModels;

public class VideoModel
{
    public VideoModel(Video video)
    {
        Title = video.Title;
        Description = video.Description;
        FilePath = video.FilePath;
    }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [StringLength(255)]
    public string FilePath { get; set; }
}