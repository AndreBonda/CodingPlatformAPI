using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
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

    [HttpPost("tournament")]
    public async Task<IActionResult> CreateTournament(CreateTournamentDto param)
    {
        // if (await _tournamentRepository.GetTournamentByName(param.TournamentName) != null)
        //     return BadRequest("Tournament name exists.");
        //
        // var user = await _userRepository.GetById(GetCurrentUserId());
        //
        // var tournament = await _tournamentRepository.InsertAsync(new Tournament()
        // {
        //     Name = param.TournamentName,
        //     MaxParticipants = param.MaxParticipants,
        //     Admin = user
        // });
        //
        // return Created(nameof(CreateTournament), new TournamentDto()
        // {
        //     Id = tournament.Id,
        //     Name = tournament.Name,
        //     MaxParticipants = tournament.MaxParticipants,
        //     DateCreated = tournament.DateCreated
        // });

        return Ok();
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
}