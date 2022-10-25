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
}