using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
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
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);

        if (tournament == null) throw new NotFoundException("Tournament does not exist");

        var adminUser = await _userRepository.GetTournamentAdminAsync(tournamentId);
        if (userId != adminUser.Id)
            throw new ForbiddenException("User is not authorized to create a challenge for this tournament");

        if (await _challengeRepository.GetActiveChallengeByTournament(tournamentId, DateTime.UtcNow) != null)
            throw new BadRequestException($@"There is a challenge in progress");

        var newChallenge = new Challenge(title, description, hours, tournament, tips);
        tournament.Challenges.Add(newChallenge);

        await _tournamentRepository.UpdateAsync(tournament);
        
        return newChallenge;
    }

    public async Task<IEnumerable<Challenge>> GetChallenges() => await _challengeRepository.GetChallengesAsync();

    public async Task<Submission> StartChallenge(long challengeId, long userId)
    {
        var challenge = await _challengeRepository.GetByIdAsync(challengeId);
        if (challenge == null) throw new NotFoundException("Challenge does not exist");

        var tournament = await _tournamentRepository.GetTournamentByChallengeAsync(challengeId);
        if (!await _tournamentRepository.IsUserSubscribedAsync(tournament.Id, userId))
            throw new BadRequestException("User is not subscribed to this tournament");

        if (!challenge.IsInProgress())
            throw new BadRequestException("Challenge is not in progress");

        var submission = await _submissionRepository.GetSubmissionByUserAndChallengeAsync(userId, challengeId);
        if (submission != null)
            throw new BadRequestException($"User has already started. Submission id: {submission.Id}");

        var user = await _userRepository.GetByIdAsync(userId);
        var newSubmission = new Submission()
        {
            DateSubmitted = null,
            Challenge = challenge,
            User = user,
            Content = string.Empty,
            TipsNumber = 0,
            Score = 0
        };
        newSubmission = await _submissionRepository.InsertAsync(newSubmission);

        return newSubmission;
    }

    public async Task<SubmissionStatus> GetSubmissionStatus(long submissionId, long userId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new NotFoundException("Submission does not exist");

        var user = await _userRepository.GetUserBySubmission(submissionId);
        if (userId != user.Id) throw new ForbiddenException("User is not authorized to this submission");

        var challenge = await _challengeRepository.GetChallengeBySubmission(submissionId);
        return new SubmissionStatus(submission, challenge, challenge.Tips);
    }

    public async Task<SubmissionStatus> AddSubmissionTip(long submissionId, long userId)
    {
        var subStatus = await GetSubmissionStatus(submissionId, userId);
        subStatus.AddTips();
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        submission.TipsNumber = (byte) subStatus.UsedTips;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }

    public async Task<SubmissionStatus> EndSubmission(long submissionId, string content, long userId)
    {
        var subStatus = await GetSubmissionStatus(submissionId, userId);
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        subStatus.EndSubmission(content);
        submission.DateSubmitted = subStatus.SubmitDate;
        submission.Content = subStatus.Content;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByChallenge(long challengeId, long userId)
    {
        var challenge = await _challengeRepository.GetByIdAsync(challengeId);
        if (challenge == null) throw new NotFoundException("Challenge does not exist");

        var admin = await _userRepository.GetAdminByChallenge(challengeId);
        if (userId != admin.Id) throw new ForbiddenException("User is not to this challenge");

        return await _submissionRepository.GetSubmissionsByChallengeAsync(challengeId);
    }

    public async Task<SubmissionStatus> EvaluateSubmission(long submissionId, int score, long userId)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null) throw new NotFoundException("Submission does not exist");

        if (!await _userRepository.IsUserAuthorizedToEvaluateSubmission(userId, submissionId))
            throw new ForbiddenException("User is not authorized to this submission");

        var challenge = await _challengeRepository.GetChallengeBySubmission(submissionId);
        var subStatus = new SubmissionStatus(submission, challenge, challenge.Tips);
        subStatus.SetScore(score);
        submission.Score = subStatus.Score;
        await _submissionRepository.UpdateAsync(submission);
        return subStatus;
    }
}