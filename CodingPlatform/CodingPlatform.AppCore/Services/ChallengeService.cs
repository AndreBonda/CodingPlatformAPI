using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Domain.Exception;
using CodingPlatform.Domain.Extensions;

namespace CodingPlatform.AppCore.Services;

public class ChallengeService : IChallengeService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IChallengeRepository _challengeRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IUserRepository _userRepository;

    public ChallengeService(ITournamentRepository tournamentRepository, IChallengeRepository challengeRepository,
        ISubmissionRepository submissionRepository, IUserRepository userRepository)
    {
        _tournamentRepository = tournamentRepository;
        _challengeRepository = challengeRepository;
        _submissionRepository = submissionRepository;
        _userRepository = userRepository;
    }

    public async Task<Challenge> CreateChallenge(long tournamentId, string title, string description,
        int hours, long userId, IEnumerable<string> tips = null)
    {
        var now = DateTime.UtcNow;
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        
        if (tournament == null) throw new NotFoundException("Tournament does not exist");
        
        var adminUser = await _tournamentRepository.GetTournamentAdminAsync(tournamentId);
        if (userId != adminUser.Id)
            throw new ForbiddenException("User can not create a challenge for this tournament");
        
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
        
        tournament.Challenges.Add(newChallenge);
        await _tournamentRepository.UpdateAsync(tournament);

        return newChallenge;
    }

    public async Task<IEnumerable<Challenge>> GetActiveChallengesByUser(long userId)
    {
        return await _challengeRepository.GetActiveChallengesByUser(userId);
    }

    public async Task<DateTime> StartChallenge(long challengeId, long userId)
    {
        var challenge = await _challengeRepository.GetByIdAsync(challengeId);
        if (challenge == null) throw new BadRequestException("Challenge does not exist");

        var tournament = await _tournamentRepository.GetTournamentByChallengeAsync(challengeId);
        if (!await _tournamentRepository.IsUserSubscribedAsync(tournament.Id, userId))
            throw new BadRequestException("User is not subscribed to this tournament");

        if (!challenge.IsInProgress())
            throw new BadRequestException("Challenge is not in progress");

        if (await _submissionRepository.GetSubmissionByUserAndChallengeAsync(userId, challengeId) != null)
            throw new BadRequestException("User has already started");

        var user = await _userRepository.GetByIdAsync(userId);
        var submission = new Submission()
        {
            DateSubmitted = null,
            Challenge = challenge,
            User = user,
            Content = string.Empty,
            TipsNumber = 0,
        };
        submission = await _submissionRepository.InsertAsync(submission);

        return submission.DateCreated;
    }
}