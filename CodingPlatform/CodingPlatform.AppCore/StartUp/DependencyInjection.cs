using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.AppCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodingPlatform.AppCore.StartUp;

public static class DependencyInjection
{
    public static IServiceCollection SetupServicesAppCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
    
}