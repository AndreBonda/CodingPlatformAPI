using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.Infrastructure.Repositories;

public class CurrentChallengeRepository : BaseRepository<CurrentChallenge>, ICurrentChallengeRepository
{
    public CurrentChallengeRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }
}