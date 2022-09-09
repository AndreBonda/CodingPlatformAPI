using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;

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

    public async Task<Tournament> Create(string tournamentName, int maxParticipants, long userId)
    {
        if (await _tournamentRepository.GetTournamentByNameAsync(tournamentName) != null)
            throw new BadRequestException("Tournament name exists.");
        
        var currentUser = await _userRepository.GetByIdAsync(userId);
        
        return await _tournamentRepository.InsertAsync(new Tournament()
        {
            Name = tournamentName,
            MaxParticipants = maxParticipants,
            Admin = currentUser
        });
    }

    public async Task<IEnumerable<TournamentInfo>> GetTournamentsInfo(TournamentFilters filters)
    {
        List<TournamentInfo> tournamentInfos = new List<TournamentInfo>();
        var tournaments = await _tournamentRepository.GetFilteredAsync(filters);

        foreach (var tour in tournaments)
        {
            var adminUserName = (await _tournamentRepository.GetTournamentAdminAsync(tour.Id)).UserName;
            var subscriberNumber = await _tournamentRepository.GetSubscriberNumberAsync(tour.Id);
            var info = new TournamentInfo(tour.Id, tour.Name, tour.MaxParticipants, adminUserName, subscriberNumber,
                tour.DateCreated);
            tournamentInfos.Add(info);
        }

        return tournamentInfos;
    }

    public async Task<UserTournamentParticipations> SubscribeUser(long tournamentId, long userId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament == null) 
            throw new NotFoundException("Tournament does not exist");
        
        if (await _tournamentRepository.IsUserSubscribedAsync(tournamentId, userId))
            throw new BadRequestException("User already subscribed");
        
        var admin = await _tournamentRepository.GetTournamentAdminAsync(tournamentId);
        if (admin.Id == userId) 
            throw new BadRequestException("An admin can't subscribe to his tournament");

        if (await _tournamentRepository.GetSubscriberNumberAsync(tournamentId) == tournament.MaxParticipants)
            throw new BadRequestException("The tournament is full");
        
        var currentUser = await _userRepository.GetByIdAsync(userId);
        return await _tournamentRepository.AddSubscriptionAsync(tournament, currentUser);
    }
}