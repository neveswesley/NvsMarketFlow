using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;
using NvsMarketFlow.Infrastructure.Repositories;

namespace NvsMarketFlow.Infrastructure.Services;

public static class ServicesExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
        AddConnectionString(services, configuration);
        AddRepositories(services);
    }

    private static void AddConnectionString(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ConnectionString")));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryWriteOnlyRepository, CategoryRepository>();
        services.AddScoped<IProductWriteOnlyRepository, ProductRepository>();
        services.AddScoped<ICategoryReadOnlyRepository, CategoryRepository>();
    }
}