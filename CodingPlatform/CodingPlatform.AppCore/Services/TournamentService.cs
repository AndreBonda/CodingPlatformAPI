using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.AppCore.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IUserRepository _userRepository;

    public TournamentService(IUserRepository userRepository, ITournamentRepository tournamentRepository, ISubmissionRepository submissionRepository)
    {
        _userRepository = userRepository;
        _tournamentRepository = tournamentRepository;
        _submissionRepository = submissionRepository;
    }

    public async Task<Tournament> Create(string tournamentName, int maxParticipants, long userId)
    {
        if (await _tournamentRepository.GetTournamentByNameAsync(tournamentName) != null)
            throw new BadRequestException("Tournament name exists.");

        var user = await _userRepository.GetByIdAsync(userId);

        return await _tournamentRepository.InsertAsync(new Tournament(tournamentName, maxParticipants, user));
    }

    public async Task SubscribeUserRefactor(long tournamentId, long userId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament == null) throw new NotFoundException(nameof(tournamentId));

        var user = await _userRepository.GetByIdAsync(userId);

        //TODO: delete
        //user.PasswordHash = new byte[0];

        tournament.AddSubscriber(user);
        await _tournamentRepository.UpdateAsync(tournament);
    }

    public async Task<IEnumerable<Tournament>> GetTournaments(TournamentSearch filters) => await _tournamentRepository.GetFilteredAsync(filters);
}