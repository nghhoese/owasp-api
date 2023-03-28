using System.ComponentModel.DataAnnotations;
using VideoSharing.CustomAttributes;

namespace VideoSharing.ApiModels;

public class VideoUploadModel
{
    [Required]
    [StringLength(100)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [AllowMp4File]
    [MaxFileSize(FileSizeInBytes: 100_000_000_000)] // 100000000 bytes = 100MB
    public IFormFile? File { get; set; }
}