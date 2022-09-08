using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IChallengeService
{
    Task<CurrentChallenge> CreateChallenge(long tournamentId, string title, string description,
        int hours, long userId, IEnumerable<string> tips = null);
}