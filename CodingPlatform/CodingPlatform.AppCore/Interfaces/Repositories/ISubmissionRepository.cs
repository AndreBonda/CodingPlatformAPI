using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<Submission> GetSubmissionByUserAndChallengeAsync(long userId, long challengeId);
}