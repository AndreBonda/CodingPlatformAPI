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
        var tournamentInfos = await _tournamentService.GetTournamentsInfo(
            new TournamentFilters(param.Take)
            {
                TournamentName = param.TournamentName
            });
        
        return Ok(tournamentInfos.Select(info => new TournamentInfoDto()
        {
            Id = info.Id,
            Name = info.Name,
            DateCreated = info.DateDateCreated,
            MaxParticipants = info.MaxParticipants,
            UserAdmin = info.UserAdmin,
            SubscriberNumber = info.SubscriberNumber,
            AvailableSeats = info.GetAvailableSeats()
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
    
    [HttpPost("tournament_subscription/{tournamentId}")]
    public async Task<IActionResult> TournamentSubscription(long tournamentId)
    {
        var result = await _tournamentService.SubscribeUser(tournamentId, GetCurrentUserId());

        return Created(nameof(TournamentSubscription), new
        {
            DateCreated = result.DateCreated
        });
    }

    [HttpGet("tournament_leaderboard/{tournamentId}")]
    public async Task<IActionResult> TournamentLeaderBoard(long tournamentId)
    {
        var positions = await _tournamentService.GetTournamentLeaderBoard(tournamentId);

        return Ok(positions.Select(p => new LeaderBoardPositionDto()
        {
            Place = p.Place,
            AveragePoints = p.AveragePoints,
            TotalPoints = p.TotalPoints,
            UserName = p.UserName
        }));
    }
}