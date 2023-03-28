using Alba;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace VideoSharing_tests.Test;

public class ExceptionHandlingTests : IClassFixture<WebAppFixture>
{
    private readonly IAlbaHost _host;

    public ExceptionHandlingTests(WebAppFixture app)
    {
        _host = app.AlbaHost;
    }

    [Fact]
    public async Task Incorrect_Request_In_Production_Environment_Should_Not_Return_Exception_Details()
    {
        // Arrange
        var response = await _host.Scenario(scenario =>
        {
            // Act
            scenario.Get.Url("/videos/a");
            scenario.IgnoreStatusCode();
        });
        var problemDetails = response.ReadAsJson<ProblemDetails>();
        // Assert
        Assert.Null(problemDetails.Detail);
    }

    [Fact]
    public async Task Incorrect_Request_In_Production_Environment_Should_Not_Return_200()
    {
        // Arrange

        var response = await _host.Scenario(scenario =>
        {
            // Act
            scenario.Get.Url("/videos/a");
            scenario.IgnoreStatusCode();
        });

        // Assert
        Assert.NotEqual(200, response.Context.Response.StatusCode);
    }
}