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
            //TODO: refactor
            //var submissions = await _dbCtx.Submissions
            //    .Where(s => s.Challenge.Tournament.Id == tournamentId)
            //    .ToListAsync();

            //return _leaderboardFactory.GetInstance(submissions);

            throw new NotImplementedException();
        }
    }
}

