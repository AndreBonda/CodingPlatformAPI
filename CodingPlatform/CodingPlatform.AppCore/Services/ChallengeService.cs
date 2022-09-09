using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;

namespace CodingPlatform.AppCore.Services;

public class ChallengeService : IChallengeService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IChallengeRepository _challengeRepository;

    public ChallengeService(ITournamentRepository tournamentRepository, IChallengeRepository challengeRepository)
    {
        _tournamentRepository = tournamentRepository;
        _challengeRepository = challengeRepository;
    }

    public async Task<Challenge> CreateChallenge(long tournamentId, string title, string description,
        int hours, long userId, IEnumerable<string> tips = null)
    {
        var now = DateTime.UtcNow;
        var tournament = await _tournamentRepository.GetById(tournamentId);
        
        if (tournament == null) throw new NotFoundException("Tournament does not exist");
        
        var adminUser = await _tournamentRepository.GetTournamentAdmin(tournamentId);
        if (userId != adminUser.Id)
            throw new ForbiddenException("You can't create a challenge for this tournament");
        
        if (await _challengeRepository.GetActiveChallengeByTournament(tournamentId, now) != null) 
            throw new BadRequestException($@"There is a challenge in progress");
        
        var newChallenge = new Challenge()
        {
            Title = title,
            Description = description,
            EndDate = DateTime.UtcNow.AddHours(hours),
            DateCreated = now,
        };
        
        if(tips != null)
            for(byte i = 0; i < tips.Count(); i++)
                newChallenge.Tips.Add(new Tip()
                {
                    Description = tips.ElementAt(i),
                    Order = (byte)(i+1),
                    DateCreated = now
                });
        
        //newChallenge = await _challengeRepository.InsertAsync(newChallenge);
        tournament.Challenges.Add(newChallenge);
        await _tournamentRepository.UpdateAsync(tournament);

        return newChallenge;
    }

    public async Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId)
    {
        return await _challengeRepository.GetActiveChallengesByUser(userId);
    }
}