using MimeDetective;
using MimeDetective.Storage;
using System.ComponentModel.DataAnnotations;

namespace VideoSharing.CustomAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public abstract class BaseAllowFileTypeAttribute : ValidationAttribute
{
    private readonly IList<Definition> _allowedDefinitions;

    public BaseAllowFileTypeAttribute(IList<Definition> allowedDefinitions)
    {
        this._allowedDefinitions = allowedDefinitions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            var file = (IFormFile)value;
            if (CheckExtension(file.FileName) && CheckMimeType(file))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Filetype is not allowed. Allowed File types: {GetAllowedExtensions(file.FileName)}");
        }
        return ValidationResult.Success;
    }

    private bool CheckMimeType(IFormFile file)
    {
        var Inspector = new ContentInspectorBuilder()
        {
            Definitions = _allowedDefinitions
        }.Build();
        var content = ContentReader.Default.ReadFromStream(file.OpenReadStream());
        var results = Inspector.Inspect(content);
        if (results.IsEmpty)
        {
            return false;
        }
        return true;
    }

    private bool CheckExtension(string fileName)
    {
        foreach (var definition in _allowedDefinitions)
        {
            var extensionsList = definition.File.Extensions;
            foreach (var extension in extensionsList)
            {
                var fileExtension = Path.GetExtension(fileName).TrimStart('.');
                if (fileExtension == extension)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private string GetAllowedExtensions(string fileName)
    {
        List<string> extensions = new();
        foreach (var definition in _allowedDefinitions)
        {
            var extensionsList = definition.File.Extensions;
            foreach (var extension in extensionsList)
            {
                extensions.Add(extension);
            }
        }

        return string.Join(", ", extensions.ToArray().Distinct().ToArray());
    }
}