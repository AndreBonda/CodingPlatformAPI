using System.Security.Authentication;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Web.DTO;
using CodingPlatform.Web.Global;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IUserService userService, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto param)
    {
        var user = await _userService.Register(param.Email, param.Username, param.Password);

        return Created(nameof(Register), new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.Username,
            DateCreated = user.CreateDate
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto param)
    {
        var jwt = await _userService.Login(param.Email, param.Password, _configuration.GetSection(Consts.JwtConfigSections).Value);

        if (string.IsNullOrEmpty(jwt)) return Forbid("Wrong password");

        return Ok(jwt);
    }

}