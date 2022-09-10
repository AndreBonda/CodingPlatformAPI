using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IChallengeRepository : IRepository<Challenge>
{
    /// <summary>
    /// Return in-progress challenges of the user.
    /// </summary>
    /// <param name="userId"></param>
    Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId);

    /// <summary>
    /// Return in progress challenge of the tournament.
    /// </summary>
    /// <param name="tournamentId"></param>
    /// <param name="now"></param>
    Task<Challenge> GetActiveChallengeByTournament(long tournamentId, DateTime? now = null);
}