using Alba;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xunit;

namespace VideoSharing_tests.Test;

public class HttpsTests : IClassFixture<WebAppFixture>
{
    private readonly IAlbaHost _host;

    public HttpsTests(WebAppFixture app)
    {
        _host = app.AlbaHost;
    }

    [Fact]
    public async Task Http_Request_Should_Return_400()
    {
        // Arrange
        var host = await Alba.AlbaHost.For<Program>(builder =>
        {
        });

        host.BeforeEach(httpContext =>
        {
            //Set Https on false
            httpContext.Request.IsHttps = false;
        });
        var response = await host.Scenario(scenario =>
        {
            // Act
            scenario.Get.Url("/videos");

            // Assert
            scenario.StatusCodeShouldBe(StatusCodes.Status400BadRequest);
        });
    }

    [Fact]
    public async Task Https_Request_Should_Return_Ok()
    {
        // Arrange
        var response = await _host.Scenario(scenario =>
        {
            scenario.ConfigureHttpContext(context =>
            {
            });
            // Act
            var request = scenario.Get.Url("/videos");

            // Assert
            scenario.StatusCodeShouldBeOk();
        });
    }
}