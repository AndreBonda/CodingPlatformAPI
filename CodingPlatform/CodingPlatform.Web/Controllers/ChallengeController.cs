using System.ComponentModel.DataAnnotations;
using CodingPlatform.AppCore.Commands;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChallengeController : CustomControllerBase
{
    private readonly IChallengeService _challengeService;

    public ChallengeController(IHttpContextAccessor httpContextAccessor, IChallengeService challengeService)
        : base(httpContextAccessor)
    {
        _challengeService = challengeService;
    }

    [HttpPost("challenge")]
    public async Task<IActionResult> CreateChallenge(CreateChallengeDto param)
    {

        var challenge = await _challengeService.CreateChallenge(new CreateChallengeCmd()
        {
            Description = param.Description,
            Hours = param.Hours,
            Tips = param.Tips,
            Title = param.Title,
            TournamentId = param.TournamentId,
            UserId = GetCurrentUserId()
        });

        return Created(nameof(CreateChallenge), new ChallengeDto()
        {
            Id = challenge.Id,
            Title = challenge.Title,
            Description = challenge.Description,
            StartDate = challenge.DateCreated,
            EndDate = challenge.EndDate,
            Tips = challenge.Tips?.Select(t => t.Description)
        });
    }

    [HttpGet("challenges/user")]
    public async Task<IActionResult> GetChallenges([FromQuery] bool onlyActive)
    {
        var userInProgressChallenges = await _challengeService.GetChallengesByUserAsync(GetCurrentUserId(), onlyActive);

        return Ok(userInProgressChallenges.Select(c => new UserChallenges
        {
            Id = c.Id,
            Title = c.Title,
            StartDate = c.DateCreated,
            EndDate = c.EndDate,
            Description = c.Description
        }));
    }

    [HttpGet("submissions_admin/{challengeId}")]
    public async Task<IActionResult> GetAdminSubmissions(long challengeId)
    {
        var submissions =
            await _challengeService.GetSubmissionsByChallenge(challengeId, GetCurrentUserId());
        return Ok(submissions.Select(s => new SubmissionDto
        {
            SubmissionId = s.Id,
            Content = s.Content,
            Score = s.Score,
            Started = s.DateCreated,
            Submitted = s.DateSubmitted
        }));
    }

    [HttpPost("submission_admin_evaluate/{submissionId}")]
    public async Task<IActionResult> EvaluateSubmission(long submissionId, [FromBody][Range(0, 5)] int score)
    {
        var subStatus = await _challengeService.EvaluateSubmission(submissionId, score, GetCurrentUserId());

        return Created(nameof(EvaluateSubmission), new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

    [HttpPost("challenge_user_start/{challengeId}")]
    public async Task<IActionResult> StartUserChallenge(long challengeId)
    {
        var submission = await _challengeService.StartChallengeAsync(challengeId, GetCurrentUserId());
        return Created(nameof(StartUserChallenge), new
        {
            IdSubmission = submission.Id,
            Started = submission.DateCreated
        });
    }

    [HttpGet("submission_user_status/{submissionId}")]
    public async Task<IActionResult> GetUserSubmissionStatus(long submissionId)
    {
        var subStatus = await _challengeService.GetSubmissionStatus(submissionId, GetCurrentUserId());
        return Ok(new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

    [HttpPost("submission_user_tip/{submissionId}")]
    public async Task<IActionResult> SubmissionUserTip(long submissionId)
    {
        var subStatus = await _challengeService.AddSubmissionTip(submissionId, GetCurrentUserId());

        return Created(nameof(SubmissionUserTip), new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

    [HttpPost("submission_user_end/{submissionId}")]
    public async Task<IActionResult> EndUserSubmission(long submissionId, [FromBody] string content)
    {
        var subStatus = await _challengeService.EndSubmission(submissionId, content, GetCurrentUserId());

        return Created(nameof(SubmissionUserTip), new SubmissionStatusDto
        {
            SubmissionId = subStatus.SubmissionId,
            Started = subStatus.StartDate,
            EndDate = subStatus.EndDate,
            Submitted = subStatus.SubmitDate,
            ChallengeTitle = subStatus.ChallengeTitle,
            ChallengeDescription = subStatus.ChallengeDescription,
            Content = subStatus.Content,
            Score = subStatus.Score,
            ChallengeTipAvailableNumber = subStatus.ChallengeTipAvailableNumber(),
            UsedTips = subStatus.GetUsedTips().ToArray(),
            RemainingTipsNumber = subStatus.RemainingTipsNumber()
        });
    }

}