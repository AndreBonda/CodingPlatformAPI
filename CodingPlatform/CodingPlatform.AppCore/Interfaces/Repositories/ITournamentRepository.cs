using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<IEnumerable<Tournament>> GetFilteredAsync(TournamentSearch f);
    Task<bool> TournamentNameExist(string name);
    /// <summary>
    /// Given a user id, returns all subscribed tournaments by the user.
    /// </summary>
    Task<IEnumerable<Tournament>> GetSubscribedTournamentsByUserAsync(long userId);
    /// <summary>
    /// Given a challenge id, returns the related tournament.
    /// </summary>
    Task<Tournament> GetTournamentByChallengeAsync(long challengeId);
}