using Alba;
using BusinesLogic.Services;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace VideoSharing_tests.Test;

public class WebAppFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;

    public async Task InitializeAsync()
    {
        var token = "";
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
                var JwtService = services.BuildServiceProvider().GetRequiredService<IJwtService>();
                token = JwtService.GetJwtToken();

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
                });

                var databaseContext = services.BuildServiceProvider().GetRequiredService<DataContext>();
                databaseContext.Database.EnsureCreated();
                if (await databaseContext.Videos.CountAsync() <= 0)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        databaseContext.Videos.Add(new Entities.Models.Video()
                        {
                            Id = i,
                            Title = "TestVid" + i,
                            Description = "TestDescription",
                            FilePath = ""
                        });
                        await databaseContext.SaveChangesAsync();
                    }
                }
            });
        });
        AlbaHost.BeforeEach(httpContext =>
        {
            //Set Https on true
            httpContext.Request.Headers["Authorization"] = "Bearer " + token;
            httpContext.Request.IsHttps = true;
        });
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }
}