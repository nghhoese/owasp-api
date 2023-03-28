using Alba;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VideoSharing.ApiModels;
using Xunit;

namespace VideoSharing_tests.Test;

public class InputValidationTests : IClassFixture<WebAppFixture>
{
    private readonly IAlbaHost _host;

    public InputValidationTests(WebAppFixture app)
    {
        _host = app.AlbaHost;
    }

    [Fact]
    public async Task Request_With_XSS_In_Body_Should_Be_Blocked()
    {
        // Arrange
        string script = "<script> alert('Hello, world!'); </script>";
        var video = new VideoUploadModel
        {
            Title = script,
            Description = "Description"
        };

        // Act
        var response = await _host.Scenario(scenario =>
        {
            scenario.Post.Json(video).ToUrl("/videos");
            scenario.IgnoreStatusCode();
        });

        // Assert
        Assert.Contains("XSS Has been detected in the request", response.ResponseBody.ReadAsText());
    }

    [Fact]
    public async Task Wrong_Extension_Should_Return_Validation_Error()
    {
        // Arrange
        VideoUploadModel uploadFileModel = null;
        string fileDir = @"../../../Files";
        string fileName = "wrong_extension_file.mp3";
        var stream = System.IO.File.OpenRead($"{fileDir}/{fileName}");

        var testFile = new FormFile(stream, 0, stream.Length, "videofile", fileName);

        uploadFileModel = new VideoUploadModel
        {
            Title = "Title",
            Description = "Description",
            File = testFile
        };

        // Act
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(uploadFileModel, new ValidationContext(uploadFileModel), validationResults, true);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Matches("Filetype is not allowed. Allowed File types:", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Wrong_File_Type_Should_Return_Validation_Error()
    {
        // Arrange
        VideoUploadModel videoUploadModel = null;
        string fileDir = @"../../../Files";
        string fileName = "fake_mp4_file.mp4";
        var stream = System.IO.File.OpenRead($"{fileDir}/{fileName}");

        var testFile = new FormFile(stream, 0, stream.Length, "videofile", fileName);

        videoUploadModel = new VideoUploadModel
        {
            Title = "Title",
            Description = "Description",
            File = testFile
        };

        // Act
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(videoUploadModel, new ValidationContext(videoUploadModel), validationResults, true);

        // Assert

        Assert.NotEmpty(validationResults);
        Assert.Matches("Filetype is not allowed. Allowed File types:", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Correct_File_Should_Return_No_Validation_Error()
    {
        // Arrange
        VideoUploadModel? uploadFileModel = null;
        string fileDir = @"../../../Files";
        string fileName = "correct_mp4_file.mp4";
        var stream = System.IO.File.OpenRead($"{fileDir}/{fileName}");

        var testFile = new FormFile(stream, 0, stream.Length, "videofile", fileName);

        uploadFileModel = new VideoUploadModel
        {
            Title = "Title",
            Description = "Description",
            File = testFile
        };
        // Act
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(uploadFileModel, new ValidationContext(uploadFileModel), validationResults, true);

        // Assert
        Assert.Empty(validationResults);
    }
}