using DataLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class DataLayerServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection serviceCollection, string connectionString)
    {
        return serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}