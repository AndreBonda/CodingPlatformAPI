using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using CodingPlatform.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(AppDbContext dbCtx) : base(dbCtx)
    {
    }

    public override async Task<Submission> GetByIdAsync(long id) =>
        await _dbCtx.Submissions
        .StandardInclude()
        .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId, bool onlySubmitted, bool excludeEvaluated, long adminId)
    {
        var submission = await _dbCtx.Submissions
            .StandardInclude()
            .Where(s => s.Challenge.Id == challengeId && s.Admin.Id == adminId)
            .ToListAsync();

        // Generalmente le submission per una challenge sono un numero limitato, quindi persisto in memoria e
        // applico i filtri con i metodi buil-in espressi nel modello senza duplicare la logica, anzichè filtrare su db.
        if (onlySubmitted)
            submission = submission.Where(s => s.IsSubmitted()).ToList();

        if (excludeEvaluated)
            submission = submission.Where(s => !s.IsEvaluated()).ToList();

        return submission;
    }
}