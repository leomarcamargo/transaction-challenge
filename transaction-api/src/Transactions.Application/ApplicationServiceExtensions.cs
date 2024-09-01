using System.Reflection;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Application.Behaviors;
using Transactions.Application.Mappings;

namespace Transactions.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile<TransactionProfile>();
        });
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}
