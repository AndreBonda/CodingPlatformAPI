using System;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly AppDbContext _dbCtx;
        private readonly ILeaderboardFactory _leaderboardFactory;

        public LeaderboardRepository(AppDbContext dbCtx, ILeaderboardFactory leaderboardFactory)
        {
            _dbCtx = dbCtx;
            _leaderboardFactory = leaderboardFactory;
        }

        public async Task<Leaderboard> GetLeaderboard(long tournamentId)
        {
            var submissions = (
                    await _dbCtx.Tournaments
                        .Include(t => t.Submissions)
                        .FirstOrDefaultAsync(t => t.Id == tournamentId)
                )?.Submissions;


            return _leaderboardFactory.Create(submissions);
        }
    }
}

