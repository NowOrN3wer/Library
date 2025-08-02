using FluentValidation;
using Library.Application.Behaviors;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Library.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 🔹 Mapster config (Assembly'deki tüm mapping'leri tara)
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        // 🔹 MediatR + Validation
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
