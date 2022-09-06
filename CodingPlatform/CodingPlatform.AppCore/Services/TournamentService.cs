using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;
using CodingPlatform.Web.Middleware;

namespace CodingPlatform.AppCore.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUserRepository _userRepository;

    public TournamentService(IUserRepository userRepository, ITournamentRepository tournamentRepository)
    {
        _userRepository = userRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<UserTournamentParticipations> SubscribeUser(long tournamentId, long userId)
    {
        var tournament = await _tournamentRepository.GetById(tournamentId);
        if (tournament == null) 
            throw new NotFoundException("Tournament does not exist");
        
        if (await _tournamentRepository.IsUserSubscribed(tournamentId, userId))
            throw new BadRequestException("User already subscribed");
        
        var admin = await _tournamentRepository.GetTournamentAdmin(tournamentId);
        if (admin.Id == userId) 
            throw new BadRequestException("An admin can't subscribe to his tournament");

        if (await _tournamentRepository.GetSubscriberNumber(tournamentId) == tournament.MaxParticipants)
            throw new BadRequestException("The tournament is full");
        
        var user = await _userRepository.GetById(userId);
        return await _tournamentRepository.AddSubscription(tournament, user);
    }
}