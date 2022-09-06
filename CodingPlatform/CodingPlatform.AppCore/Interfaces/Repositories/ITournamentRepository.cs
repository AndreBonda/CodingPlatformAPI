using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<IEnumerable<Tournament>> GetFiltered(TournamentFilters f);
    Task<Tournament> GetTournamentByName(string name);
    Task<bool> IsUserSubscribed(long tournamentId, long userId);
    Task<User> GetTournamentAdmin(long tournamentId);
    Task<UserTournamentParticipations> AddSubscription(Tournament tournament, User user);
    Task<long> GetSubscriberNumber(long tournamentId);
}