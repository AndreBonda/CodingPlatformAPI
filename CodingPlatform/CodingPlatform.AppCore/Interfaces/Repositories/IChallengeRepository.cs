using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IChallengeRepository : IRepository<Challenge>
{
    Task<IEnumerable<Challenge>> GetChallengesByUser(long userId, bool onlyActive);
    /// <summary>
    /// Return in progress challenge of the tournament.
    /// </summary>
    /// <param name="tournamentId"></param>
    /// <param name="now"></param>
    Task<Challenge> GetActiveChallengeByTournament(long tournamentId, DateTime? now = null);
    Task<Challenge> GetChallengeBySubmission(long submissionId);
}