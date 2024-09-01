using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Domain.Repositories;
using Transactions.Infrastructure.Data.Contexts;
using Transactions.Infrastructure.Data.Repositories;

namespace Transactions.Infrastructure.Data;

public static class DataServiceExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TransactionConnection")));
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        return services;
    }
}
