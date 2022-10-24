using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface ITournamentRepository : IRepositoryRefactor<Tournament>
{
    //Task<Tournament> GetTournamentByIdAsync(long tournamentId);
    Task<IEnumerable<Tournament>> GetFilteredAsync(TournamentSearch f);
    Task<bool> TournamentNameExist(string name);
    //Task<bool> IsUserSubscribedAsync(long tournamentId, long userId);
    //Task<UserTournamentParticipations> AddSubscriptionAsync(Tournament tournament, User user);
    Task<int> GetSubscriberNumberAsync(long tournamentId);
    Task<Tournament> GetTournamentByChallengeAsync(long challengeId);
}