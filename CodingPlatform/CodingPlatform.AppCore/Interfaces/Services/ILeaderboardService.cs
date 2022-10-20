using System;

using CodingPlatform.Domain;
using static CodingPlatform.Domain.Leaderboard;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface ILeaderboardService
{
    Task<Dictionary<int, Placement>> GetLeaderboardPlacements(long tournamentId, Sorting sortingType, bool sortingAsc = false);
}