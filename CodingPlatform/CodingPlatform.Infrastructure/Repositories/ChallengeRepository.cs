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
    
    public async Task<Challenge> GetActiveChallengeByTournament(long tournamentId, DateTime? now = null)
    {
        now ??= DateTime.UtcNow;
        
        return await dbCtx.Challenges.FirstOrDefaultAsync(c =>
            c.Tournament.Id == tournamentId &&
            c.DateCreated <= now && c.EndDate >= now);
    }

    public async Task<IEnumerable<Tip>> GetChallengeTips(long challengeId)
    { 
        return (
            await dbCtx.Challenges
                .Include(c => c.Tips)
                .FirstAsync(c => c.Id == challengeId)
            ).Tips;
    }
}