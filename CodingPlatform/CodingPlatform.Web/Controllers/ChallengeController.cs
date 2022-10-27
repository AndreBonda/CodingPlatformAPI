using System.ComponentModel.DataAnnotations;
using CodingPlatform.AppCore.Commands;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Web.DTO;
using CodingPlatform.Web.Extensions;
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

        //TODO: cosa mettere nel primo parametro in Created?
        return Created(nameof(CreateChallenge), new ChallengeDto()
        {
            Id = challenge.Id,
            Title = challenge.Title,
            Description = challenge.Description,
            StartDate = challenge.CreateDate,
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
            StartDate = c.CreateDate,
            EndDate = c.EndDate,
            Description = c.Description
        }));
    }

    [HttpPost("challenge/start/{challengeId}")]
    public async Task<IActionResult> ChallengeStart(long challengeId)
    {
        var submission = await _challengeService.StartChallengeAsync(challengeId, GetCurrentUserId());

        return Created(nameof(ChallengeStart), submission.ToDTO());
    }

    [HttpGet("challenge/status/{challengeId}")]
    public async Task<IActionResult> ChallengeStatus(long challengeId)
    {
        var submission = await _challengeService.GetSubmissionStatusAsync(challengeId, GetCurrentUserId());

        return Created(nameof(ChallengeStart), submission.ToDTO());
    }

    [HttpPost("challenge/tip")]
    public async Task<IActionResult> ChallengeTip(long challengeId)
    {
        var tip = await _challengeService.RequestNewTipAsync(challengeId, GetCurrentUserId());

        return Created(nameof(Challenge), new TipDto
        {
            Description = tip.Description,
            Order = tip.Order
        });
    }

    [HttpPost("challenge/end/{challengeId}")]
    public async Task<IActionResult> ChallengeEnd([FromBody] ChallengeEnd param)
    {
        var submission = await _challengeService.EndChallengeAsync(param.ChallengeId, param.Content,
            GetCurrentUserId());

        return Created(nameof(ChallengeEnd), submission.ToDTO());
    }
}