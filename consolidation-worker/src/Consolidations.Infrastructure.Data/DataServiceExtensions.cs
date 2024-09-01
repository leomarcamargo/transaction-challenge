using Consolidations.Domain.Repositories;
using Consolidations.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Consolidations.Infrastructure.Data;

public static class DataServiceExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConsolidatedTransactionRepository, ConsolidatedTransactionRepository>();
        return services;
    }
}
