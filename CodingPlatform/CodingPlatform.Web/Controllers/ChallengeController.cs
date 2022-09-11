using System.ComponentModel.DataAnnotations;
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
    
    [HttpGet("challenges")]
    public async Task<IActionResult> GetChallenges()
    {
        var userInProgressChallenges = await _challengeService.GetChallenges();
        
        return Ok(userInProgressChallenges.Select(c => new InfoInProgressChallengeDto
        {
            ChallengeId = c.Id,
            ChallengeName = c.Title,
            DateStart = c.DateCreated,
            DateEnd = c.EndDate,
            TournamentName = c.Tournament.Name,
            Admin = c.Tournament.Admin.UserName
        }));
    }

    [HttpPost("challenge_admin")]
    public async Task<IActionResult> CreateChallenge(CreateChallengeDto param)
    {
        var challenge = await _challengeService.CreateChallenge(param.TournamentId, param.Title, param.Description,
            param.Hours, GetCurrentUserId(), param.Tips);
        
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
    public async Task<IActionResult> EvaluateSubmission(long submissionId, [FromBody][Range(0,5)] int score)
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
        var submission = await _challengeService.StartChallenge(challengeId, GetCurrentUserId());
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