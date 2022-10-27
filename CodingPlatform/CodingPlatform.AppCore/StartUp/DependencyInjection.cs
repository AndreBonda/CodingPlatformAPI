using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.AppCore.Services;
using CodingPlatform.Domain.Factories;
using CodingPlatform.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodingPlatform.AppCore.StartUp;

public static class DependencyInjection
{
    public static IServiceCollection SetupServicesAppCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITournamentService, TournamentService>();
        services.AddScoped<IChallengeService, ChallengeService>();
        services.AddScoped<ILeaderboardService, LeaderboardService>();
        services.AddScoped<ISubmissionService, SubmissionService>();

        services.AddScoped<ILeaderboardFactory, LeaderboardFactory>();
        return services;
    }

}