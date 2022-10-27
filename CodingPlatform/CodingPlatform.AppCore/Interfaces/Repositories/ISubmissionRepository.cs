using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId, bool onlySubmitted, bool excludeEvaluated, long adminId);
}