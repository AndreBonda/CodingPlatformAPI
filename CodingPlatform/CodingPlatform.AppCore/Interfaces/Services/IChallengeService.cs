using CodingPlatform.Domain;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IChallengeService
{
    Task<Challenge> CreateChallenge(long tournamentId, string title, string description,
        int hours, long userId, IEnumerable<string> tips = null);
    
    Task<IEnumerable<Challenge>> GetChallenges();
    Task<Submission> StartChallenge(long challengeId, long userId);
    Task<SubmissionStatus> GetSubmissionStatus(long submissionId, long userId);
    Task<SubmissionStatus> AddSubmissionTip(long submissionId, long userId);
    Task<SubmissionStatus> EndSubmission(long submissionId, string content, long userId);
    Task<IEnumerable<Submission>> GetSubmissionsByChallenge(long challengeId, long userId);
    Task<SubmissionStatus> EvaluateSubmission(long submissionId, int score, long userId);
}