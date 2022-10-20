using System;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.AppCore.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardRepo;
    private readonly ITournamentRepository _tournamentRepo;

    public LeaderboardService(ILeaderboardRepository leaderboardRepo, ITournamentRepository tournamentRepository)
    {
        _leaderboardRepo = leaderboardRepo;
        _tournamentRepo = tournamentRepository;
    }

    public async Task<Dictionary<int, Placement>> GetLeaderboardPlacements(long tournamentId, Leaderboard.Sorting sortingType, bool sortingAsc = false)
    {
        if (!await _tournamentRepo.ExistAsync(tournamentId)) throw new NotFoundException(nameof(tournamentId));
        var leaderboard = await _leaderboardRepo.GetLeaderboard(tournamentId);
        return leaderboard.GetSortedPlacements(sortingType, sortingAsc);
    }
}