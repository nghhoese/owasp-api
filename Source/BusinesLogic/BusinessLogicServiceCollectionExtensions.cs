using BusinesLogic.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class BusinessLogicServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection, string connectionString)
    {
        return serviceCollection.AddDataLayer(connectionString).AddTransient<IVideoService, VideoService>().AddTransient<IJwtService, JwtService>().AddTransient<IStorageService, StorageService>();
    }
}