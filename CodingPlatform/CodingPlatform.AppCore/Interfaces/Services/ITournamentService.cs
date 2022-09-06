using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface ITournamentService
{
    Task<UserTournamentParticipations> SubscribeUser(long tournamentId, long userId);
}