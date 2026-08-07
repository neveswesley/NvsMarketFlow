using Microsoft.Extensions.DependencyInjection;
using NvsMarketFlow.Application.Services.BarCode;
using MediatR;
using NvsMarketFlow.Application.UseCases.Category.Commands;


namespace NvsMarketFlow.Application.Services;

public static class ServicesExtensions
{

    public static IServiceCollection ConfigurationApplicationApp(this IServiceCollection services)
    {
        services.AddScoped<IBarCodeService, BarCodeService>();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCategoryCommand).Assembly);
        });
        
        
        return services;
    }
    
}