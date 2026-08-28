using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NvsMarketFlow.Application.Behaviors;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.Services;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;
using NvsMarketFlow.Infrastructure.Interceptors;
using NvsMarketFlow.Infrastructure.Repositories;
using NvsMarketFlow.Infrastructure.Security;

namespace NvsMarketFlow.Infrastructure.Services;

public static class ServicesExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
        AddConnectionString(services, configuration);
        AddRepositories(services);
        AddPasswordHasher(services);
    }

    private static void AddConnectionString(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        
        services.AddScoped<ICategoryWriteOnlyRepository, CategoryRepository>();
        services.AddScoped<ICategoryReadOnlyRepository, CategoryRepository>();
        
        services.AddScoped<IProductWriteOnlyRepository, ProductRepository>();
        services.AddScoped<IProductReadOnlyRepository, ProductRepository>();
        
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        
        services.AddScoped<IBrandReadOnlyRepository, BrandRepository>();
        services.AddScoped<IBrandWriteOnlyRepository, BrandRepository>();
        
        services.AddScoped<IStockMovementReadOnlyRepository, StockMovementRepository>();
        services.AddScoped<IStockMovementWriteOnlyRepository, StockMovementRepository>();
        
        services.AddScoped<ISupplierReadOnlyRepository, SupplierRepository>();
        services.AddScoped<ISupplierWriteOnlyRepository, SupplierRepository>();
        
        services.AddScoped<IPurchaseReadOnlyRepository, PurchaseRepository>();
        services.AddScoped<IPurchaseWriteOnlyRepository, PurchaseRepository>();
        
        services.AddScoped<ICashRegisterReadOnlyRepository, CashRegisterRepository>();
        services.AddScoped<ICashRegisterWriteOnlyRepository, CashRegisterRepository>();
        
        services.AddScoped<ICashMovementReadOnlyRepository, CashMovementRepository>();
        services.AddScoped<ICashMovementWriteOnlyRepository, CashMovementRepository>();
        
        services.AddScoped<ISaleReadOnlyRepository, SaleRepository>();
        services.AddScoped<ISaleWriteOnlyRepository, SaleRepository>();
        
        services.AddScoped<INotificationReadOnlyRepository, NotificationRepository>();
        services.AddScoped<INotificationWriteOnlyRepository, NotificationRepository>();
        
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<AuditSaveChangesInterceptor>();
        
        services.AddScoped<IAuditLogReadOnlyRepository, AuditLogRepository>();
        
        
    }

    private static void AddPasswordHasher(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
    }
}