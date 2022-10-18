using System;
namespace CodingPlatform.AppCore.Interfaces.Repositories
{
    public interface ILeaderboardRepository
    {
        Task<Leaderboard> GetLeaderboard(long tournamentId);
    }
}

