using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class ChallengeRepository : BaseRepository<Challenge>, IChallengeRepository
{
    public ChallengeRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<IEnumerable<Challenge>> GetChallengesByUser(long userId, bool onlyActive)
    {
        var challenges = await _dbCtx.Tournaments
            .Where(t => t.SubscribedUser.Any(s => s.User.Id == userId))
            .SelectMany(t => t.Challenges)
            .ToListAsync();

        if (onlyActive)
            challenges = challenges.Where(c => c.IsActive()).ToList();

        return challenges;
    }

    public async Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId)
    {
        //var now = DateTime.UtcNow;

        //var tournamentIds = await _dbCtx.UserTournamentParticipations
        //    .Where(up => up.User.Id == userId)
        //    .Select(up => up.Tournament.Id)
        //    .ToListAsync();

        //return await _dbCtx.Challenges
        //    .Include(c => c.Tournament)
        //    .Where(c => c.DateCreated <= now && c.EndDate >= now &&
        //                tournamentIds.Contains(c.Tournament.Id))
        //    .ToListAsync();

        throw new NotImplementedException();
    }

    public async Task<Challenge> GetActiveChallengeByTournament(long tournamentId, DateTime? now = null)
    {
        //now ??= DateTime.UtcNow;

        //return await _dbCtx.Challenges.FirstOrDefaultAsync(c =>
        //    c.Tournament.Id == tournamentId &&
        //    c.DateCreated <= now && c.EndDate >= now);

        throw new NotImplementedException();
    }

    public async Task<Challenge> GetChallengeBySubmission(long submissionId)
    {
        return (
            await _dbCtx.Submissions
                .Include(s => s.Challenge).ThenInclude(c => c.Tips)
                .FirstAsync(s => s.Id == submissionId)
        ).Challenge;
    }
}