using CodingPlatform.AppCore.Commands;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IChallengeService
{
    //Task<Challenge> CreateChallenge(long tournamentId, string title, string description,
    //    int hours, long userId, IEnumerable<string> tips = null);

    Task<Challenge> CreateChallenge(CreateChallengeCmd cmd);
    Task<IEnumerable<Challenge>> GetChallengesByUserAsync(long userId, bool onlyActive);

    Task<Submission> StartChallengeAsync(long challengeId, long userId);
    Task<SubmissionStatus> GetSubmissionStatus(long submissionId, long userId);
    Task<SubmissionStatus> AddSubmissionTip(long submissionId, long userId);
    Task<SubmissionStatus> EndSubmission(long submissionId, string content, long userId);
    Task<IEnumerable<Submission>> GetSubmissionsByChallenge(long challengeId, long userId);
    Task<SubmissionStatus> EvaluateSubmission(long submissionId, int score, long userId);
}