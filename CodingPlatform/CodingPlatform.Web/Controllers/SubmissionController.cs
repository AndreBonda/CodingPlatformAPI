using System.ComponentModel.DataAnnotations;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Web.DTO;
using CodingPlatform.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubmissionController : CustomControllerBase
{
    private readonly ISubmissionService _submissionService;

    public SubmissionController(IHttpContextAccessor httpContextAccessor, ISubmissionService submissionService)
        : base(httpContextAccessor)
    {
        _submissionService = submissionService;
    }

    [HttpGet("submissions/admin/{challengeId}")]
    public async Task<IActionResult> GetSubmissions(long challengeId, [FromQuery] SearchSubmissionDTO param)
    {
        var submissions = await _submissionService.GetSubmissionsByChallengeAsync(challengeId, param.OnlySubmitted, param.ExcludeEvaluated, GetCurrentUserId());

        return Ok(submissions.Select(s => s.ToDTO()));
    }

    [HttpPut("submission/evaluate/{submissionId}")]
    public async Task<IActionResult> EvaluateSubmission(long submissionId, [FromBody][Range(0, 5)] int score)
    {
        var submission = await _submissionService.EvaluateSubmission(submissionId, score, GetCurrentUserId());

        return Ok(submission.ToDTO());
    }
}

