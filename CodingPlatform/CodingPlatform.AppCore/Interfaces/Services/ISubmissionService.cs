using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface ISubmissionService
{
    Task<Submission> EvaluateSubmission(long submissionId, int score, long userId);
    Task<IEnumerable<Submission>> GetSubmissionsByChallengeAsync(long challengeId, bool onlySubmitted, bool excludeEvaluated, long userId);
}