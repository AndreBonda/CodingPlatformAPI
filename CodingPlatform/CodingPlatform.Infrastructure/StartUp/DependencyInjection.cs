using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CodingPlatform.Infrastructure.StartUp;

public static class DependencyInjection
{
    public static IServiceCollection SetupServicesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}