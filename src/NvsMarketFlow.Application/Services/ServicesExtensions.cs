using Microsoft.Extensions.DependencyInjection;
using NvsMarketFlow.Application.Services.BarCode;
using MediatR;
using NvsMarketFlow.Application.UseCases.Category.Commands;
using NvsMarketFlow.Application.UseCases.Category.Validators;
using FluentValidation;
using NvsMarketFlow.Application.Behaviors;


namespace NvsMarketFlow.Application.Services;

public static class ServicesExtensions
{

    public static IServiceCollection ConfigurationApplicationApp(this IServiceCollection services)
    {
        services.AddScoped<IBarCodeService, BarCodeService>();
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(
                typeof(ValidationBehavior<,>).Assembly);

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(
            typeof(CreateCategoryValidator).Assembly);
        
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        
        return services;
    }
    
    
}