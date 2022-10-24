using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface ITournamentService
{
    Task<Tournament> Create(string tournamentName, int maxParticipants, long userId);
    Task<IEnumerable<Tournament>> GetTournaments(TournamentSearch filters);
    Task SubscribeUserRefactor(long tournamentId, long userId);
}