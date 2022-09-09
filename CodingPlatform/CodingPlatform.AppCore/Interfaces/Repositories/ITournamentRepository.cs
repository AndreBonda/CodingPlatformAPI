using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<IEnumerable<Tournament>> GetFilteredAsync(TournamentFilters f);
    Task<Tournament> GetTournamentByNameAsync(string name);
    Task<bool> IsUserSubscribedAsync(long tournamentId, long userId);
    Task<User> GetTournamentAdminAsync(long tournamentId);
    Task<UserTournamentParticipations> AddSubscriptionAsync(Tournament tournament, User user);
    Task<int> GetSubscriberNumberAsync(long tournamentId);
    Task<Tournament> GetTournamentByChallengeAsync(long challengeId);
}