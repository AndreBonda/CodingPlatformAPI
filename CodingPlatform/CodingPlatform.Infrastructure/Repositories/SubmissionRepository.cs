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

    public async Task<Challenge> GetChallengeBySubmission(long submissionId)
    {
        return (
            await dbCtx.Submissions
                .Include(s => s.Challenge).ThenInclude(c => c.Tips)
                .FirstAsync(s => s.Id == submissionId)
        ).Challenge;
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId)
    {
        return await dbCtx.Submissions
            .Where(s => s.Challenge.Id == challengeId)
            .ToListAsync();
    }
}