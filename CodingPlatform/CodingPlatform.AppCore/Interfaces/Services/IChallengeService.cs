using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IChallengeService
{
    Task<Challenge> CreateChallenge(long tournamentId, string title, string description,
        int hours, long userId, IEnumerable<string> tips = null);

    /// <summary>
    /// Given an user id returns his available challenges
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Returns challenges</returns>
    Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId);

    Task<Submission> StartChallenge(long challengeId, long userId);
}