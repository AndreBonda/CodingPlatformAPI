using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.AppCore.Services;

public class ChallengeService : IChallengeService
{
    private readonly ICurrentChallengeRepository _currentChallengeRepository;
    private readonly ITournamentRepository _tournamentRepository;

    public ChallengeService(ICurrentChallengeRepository currentChallengeRepository, ITournamentRepository tournamentRepository)
    {
        _currentChallengeRepository = currentChallengeRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<CurrentChallenge> CreateChallenge(long tournamentId, string title, string description,
        int hours, long userId, IEnumerable<string> tips = null)
    {
        var now = DateTime.UtcNow;
        var tournament = await _tournamentRepository.GetById(tournamentId);
        
        if (tournament == null) throw new NotFoundException("Tournament does not exist");

        var adminUser = await _tournamentRepository.GetTournamentAdmin(tournamentId);
        if (userId != adminUser.Id)
            throw new ForbiddenException("You can't create a challenge for this tournament");

        var currentChallenge = await _tournamentRepository.GetCurrentChallenge(tournamentId);

        if (currentChallenge != null && currentChallenge.EndDate > now)
            throw new BadRequestException("There is a challenge in progress");

        currentChallenge = new CurrentChallenge()
        {
            Title = title,
            Description = description,
            EndDate = DateTime.UtcNow.AddHours(hours),
            DateCreated = now
        };

        foreach (var tip in tips)
        {
            currentChallenge.Tips.Add(new Tip()
            {
                Description = tip,
                DateCreated = now
            });
        }

        tournament.CurrentChallenge = currentChallenge;
        tournament = await _tournamentRepository.UpdateAsync(tournament);

        return tournament.CurrentChallenge;
    }
}