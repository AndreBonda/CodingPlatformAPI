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
        var query = dbCtx.Set<UserTournamentParticipations>()
            .Include(up => up.User)
            .Include(up => up.Tournament)
            .ThenInclude(t => t.Challenges)
            .Where(up => up.User.Id == userId)
            .Select(up => up.Tournament);
        
        var tournament = await query.ToListAsync();
        
        throw new NotImplementedException();
    }
}