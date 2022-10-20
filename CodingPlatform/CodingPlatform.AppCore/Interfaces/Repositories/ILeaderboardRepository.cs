using System;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ILeaderboardRepository
{
    Task<Leaderboard> GetLeaderboard(long tournamentId);
}