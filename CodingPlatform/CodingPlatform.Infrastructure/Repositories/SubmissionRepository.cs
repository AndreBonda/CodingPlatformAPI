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
        return await dbCtx.Submissions.SingleOrDefaultAsync(
            s => s.User.Id == userId && s.Challenge.Id == challengeId);
    }
}