using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface ITournamentService
{
    Task<Tournament> Create(string tournamentName, int maxParticipants, long userId);
    Task<IEnumerable<TournamentInfo>> GetTournamentsInfo(TournamentFilters filters);
    Task<UserTournamentParticipations> SubscribeUser(long tournamentId, long userId);
}