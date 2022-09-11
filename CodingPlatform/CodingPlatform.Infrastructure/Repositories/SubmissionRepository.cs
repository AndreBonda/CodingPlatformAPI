using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<Submission> GetSubmissionByUserAndChallengeAsync(long userId, long challengeId)
    {
        return await dbCtx.Submissions.FirstOrDefaultAsync(
            s => s.User.Id == userId && s.Challenge.Id == challengeId);
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId)
    {
        return await dbCtx.Submissions
            .Where(s => s.Challenge.Id == challengeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetSubmissionByTournament(long tournamentId)
    {
        var tournament = await dbCtx.Tournaments
            .Include(t => t.Challenges)
            .ThenInclude(c => c.Submissions)
            .ThenInclude(s => s.User)
            .FirstAsync(t => t.Id == tournamentId);

        return tournament.Challenges.SelectMany(c => c.Submissions);
    }
}