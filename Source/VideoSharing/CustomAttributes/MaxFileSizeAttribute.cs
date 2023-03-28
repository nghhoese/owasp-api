using System.ComponentModel.DataAnnotations;

namespace VideoSharing.CustomAttributes;

public class MaxFileSizeAttribute : ValidationAttribute
{
    public MaxFileSizeAttribute(long FileSizeInBytes)
    {
        this.FileSizeInBytes = FileSizeInBytes;
    }

    public long FileSizeInBytes { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            var file = (IFormFile)value;
            if (file.Length < FileSizeInBytes)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"File is too large: {file.Length}. Please use a file smaller than {FileSizeInBytes}");
        }
        return ValidationResult.Success;
    }
}