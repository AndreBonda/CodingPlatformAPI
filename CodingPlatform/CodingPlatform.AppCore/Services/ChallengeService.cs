using CodingPlatform.AppCore.Commands;
using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Exception;
using CodingPlatform.Domain.Extensions;

namespace CodingPlatform.AppCore.Services;

public class ChallengeService : IChallengeService
{
    private readonly ITournamentRepository _tournamentRepo;
    private readonly IChallengeRepository _challengeRepo;
    private readonly ISubmissionRepository _submissionRepo;
    private readonly IUserRepository _userRepo;

    public ChallengeService(ITournamentRepository tournamentRepository, IChallengeRepository challengeRepository,
        ISubmissionRepository submissionRepository, IUserRepository userRepository)
    {
        _tournamentRepo = tournamentRepository;
        _challengeRepo = challengeRepository;
        _submissionRepo = submissionRepository;
        _userRepo = userRepository;
    }

    public async Task<Challenge> CreateChallenge(CreateChallengeCmd cmd)
    {
        var tournament = await _tournamentRepo.GetByIdAsync(cmd.TournamentId);
        if (tournament == null) throw new NotFoundException("Tournament not found");

        var user = await _userRepo.GetByIdAsync(cmd.UserId);
        var challenge = Challenge.CreateNew(cmd.Title,
                cmd.Description,
                cmd.Hours,
                cmd.Tips);
        tournament.AddChallenge(challenge, user);
        await _tournamentRepo.UpdateAsync(tournament);

        return challenge;
    }

    public async Task<IEnumerable<Challenge>> GetChallengesByUserAsync(long userId, bool onlyActive)
    {
        var subscribedTournaments = await _tournamentRepo.GetSubscribedTournamentsByUserAsync(userId);
        if (subscribedTournaments.Count() == 0) return Enumerable.Empty<Challenge>();

        var challenges = subscribedTournaments.SelectMany(t => t.Challenges);

        if (onlyActive)
            challenges = challenges.Where(c => c.IsActive());

        return challenges;
    }

    public async Task<Submission> StartChallengeAsync(long challengeId, long userId)
    {
        var tournament = await _tournamentRepo.GetTournamentByChallengeAsync(challengeId);
        if (tournament == null) throw new NotFoundException("Challenge not found");

        var user = await _userRepo.GetByIdAsync(userId);
        var submission = tournament.StartChallenge(user);
        await _tournamentRepo.UpdateAsync(tournament);
        return submission;
    }

    public async Task<Submission> GetSubmissionStatusAsync(long challengeId, long userId)
    {
        var tournament = await _tournamentRepo.GetTournamentByChallengeAsync(challengeId);
        if (tournament == null) throw new NotFoundException("Challenge not found");

        var user = await _userRepo.GetByIdAsync(userId);
        var submission = tournament.GetSubmissionStatus(user, challengeId);
        return submission;
    }

    public async Task<Tip> RequestNewTipAsync(long challengeId, long userId)
    {
        //var tournament = await _tournamentRepo.GetTournamentByChallengeAsync(challengeId);
        //if (tournament == null) throw new NotFoundException("Challenge not found");

        //var user = await _userRepo.GetByIdAsync(userId);
        //var submission = tournament.GetSubmissionStatus(user, challengeId);
        var submission = await GetSubmissionStatusAsync(challengeId, userId);
        submission.RequestNewTip(userId);
        await _submissionRepo.UpdateAsync(submission);
        return submission.GetLastAvailableTips();
    }

    public async Task<Submission> EndChallengeAsync(long challengeId, string content, long userId)
    {
        //var tournament = await _tournamentRepo.GetTournamentByChallengeAsync(challengeId);
        //if (tournament == null) throw new NotFoundException("Challenge not found");

        //var user = await _userRepo.GetByIdAsync(userId);
        //var submission = tournament.GetSubmissionStatus(user, challengeId);
        var submission = await GetSubmissionStatusAsync(challengeId, userId);
        submission.EndSubmission(content, userId);
        await _submissionRepo.UpdateAsync(submission);
        return submission;
    }
}