using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<Submission> GetSubmissionByUserAndChallengeAsync(long userId, long challengeId);
    Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId);
}