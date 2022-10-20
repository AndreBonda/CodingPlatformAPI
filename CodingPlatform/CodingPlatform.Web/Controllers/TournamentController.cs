using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TournamentController : CustomControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentController(IHttpContextAccessor httpCtxAccessor, ITournamentService tournamentService)
        : base(httpCtxAccessor)
    {
        _tournamentService = tournamentService;
    }

    [HttpGet("tournaments")]
    public async Task<IActionResult> GetTournaments([FromQuery] SearchTournamentDto param)
    {
        var search = new TournamentSearch()
        {
            TournamentName = param.TournamentName
        };

        var tournaments = await _tournamentService.GetTournaments(search);

        return Ok(tournaments.Select(t => new TournamentDto()
        {
            Id = t.Id,
            Name = t.Name,
            DateCreated = t.DateCreated,
            MaxParticipants = t.MaxParticipants,
            UsernameAdmin = t.Admin.Username,
            SubscriberNumber = t.SubscribedNumber,
            AvailableSeats = t.AvailableSeats
        }));
    }

    [HttpPost("tournament")]
    public async Task<IActionResult> CreateTournament(CreateTournamentDto param)
    {
        var result = await _tournamentService.Create(param.TournamentName, param.MaxParticipants,
            GetCurrentUserId());

        return Created(nameof(CreateTournament), new TournamentDto()
        {
            Id = result.Id,
            Name = result.Name,
            MaxParticipants = result.MaxParticipants,
            DateCreated = result.DateCreated
        });
    }

    [HttpPost("subscription/{tournamentId}")]
    public async Task<IActionResult> Subscription(long tournamentId)
    {
        await _tournamentService.SubscribeUserRefactor(tournamentId, GetCurrentUserId());
        return Created(nameof(Subscription), "Subscription created");
    }
}