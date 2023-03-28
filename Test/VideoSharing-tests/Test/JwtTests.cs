using Alba;
using BusinesLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace VideoSharing_tests.Test;

public class JwtTests : IClassFixture<WebAppFixture>
{
    private readonly IAlbaHost _host;

    public JwtTests(WebAppFixture app)
    {
        _host = app.AlbaHost;
    }

    [Fact]
    public async Task False_Token_Should_Return_401()
    {
        // Arrange

        var response = await _host.Scenario(scenario =>
        {
            scenario.ConfigureHttpContext(context =>
            {
                context.Request.Headers["Authorization"] = "false_token";
            });
            // Act
            scenario.Get.Url("/videos");

            // Assert
            scenario.StatusCodeShouldBe(StatusCodes.Status401Unauthorized);
        });
    }

    [Fact]
    public async Task Legit_Token_Should_Return_Ok()
    {
        // Arrange
        var token = "";
        var host = await Alba.AlbaHost.For<Program>(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var JwtService = services.BuildServiceProvider().GetRequiredService<IJwtService>();
                token = JwtService.GetJwtToken();
            });
        });
        host.BeforeEach(httpContext =>
        {
            httpContext.Request.IsHttps = true;
            httpContext.Request.Headers["Authorization"] = "Bearer " + token;
        });
        var response = await host.Scenario(scenario =>
           {
               // Act
               scenario.Get.Url("/videos");

               // Assert
               scenario.StatusCodeShouldBeOk();
           });
    }
}