using System.ComponentModel.DataAnnotations;

namespace VideoSharing.CustomAttributes
{
    public class StandardMimeTypeDetectionAttribute : ValidationAttribute
    {
        public StandardMimeTypeDetectionAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var file = (IFormFile)value;
                if (file.ContentType == "video/mp4")
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"File is not mp4");
            }
            return ValidationResult.Success;
        }
    }
}
