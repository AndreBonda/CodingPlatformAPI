using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public async Task<Submission> GetSubmissionByUserAndChallengeAsync(long userId, long challengeId)
    {
        return await _dbCtx.Submissions.FirstOrDefaultAsync(
            s => s.User.Id == userId && s.Challenge.Id == challengeId);
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId)
    {
        return await _dbCtx.Submissions
            .Where(s => s.Challenge.Id == challengeId)
            .ToListAsync();
    }
}