using System.ComponentModel.DataAnnotations;
using VideoSharing.CustomAttributes;

namespace VideoSharing.ApiModels
{
    public class StandardVideoUploadModel
    {
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StandardMimeTypeDetection]
        [MaxFileSize(FileSizeInBytes: 100_000_000_000)] // 100000000 bytes = 100MB
        public IFormFile? File { get; set; }
    }
}
