using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class ChallengeRepository : BaseRepository<Challenge>, IChallengeRepository
{
    public ChallengeRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId)
    {
        var now = DateTime.UtcNow;
        
        var tournamentIds = await dbCtx.UserTournamentParticipations
            .Where(up => up.User.Id == userId)
            .Select(up => up.Tournament.Id)
            .ToListAsync();

        return await dbCtx.Challenges
            .Include(c => c.Tournament)
            .Where(c => c.DateCreated <= now && c.EndDate >= now &&
                        tournamentIds.Contains(c.Tournament.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Challenge>> GetChallengesAsync()
    {
        return await dbCtx.Challenges
            .Include(c => c.Tournament).ThenInclude(t => t.Admin)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync();
    }

    public async Task<Challenge> GetActiveChallengeByTournament(long tournamentId, DateTime? now = null)
    {
        now ??= DateTime.UtcNow;
        
        return await dbCtx.Challenges.FirstOrDefaultAsync(c =>
            c.Tournament.Id == tournamentId &&
            c.DateCreated <= now && c.EndDate >= now);
    }
    
    public async Task<Challenge> GetChallengeBySubmission(long submissionId)
    {
        return (
            await dbCtx.Submissions
                .Include(s => s.Challenge).ThenInclude(c => c.Tips)
                .FirstAsync(s => s.Id == submissionId)
        ).Challenge;
    }
}