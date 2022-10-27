using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Interfaces.Utility;
using CodingPlatform.Infrastructure.Repositories;
using CodingPlatform.Infrastructure.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace CodingPlatform.Infrastructure.StartUp;

public static class DependencyInjection
{
    public static IServiceCollection SetupServicesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        services.AddScoped<IChallengeRepository, ChallengeRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        services.AddSingleton<IAuthenticationProvider, SHA512AuthenticationProvider>();

        return services;
    }
}