using System.Security.Authentication;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Web.DTO;
using CodingPlatform.Web.Global;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    
    public UserController(IUserRepository userRepository, IUserService userService, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _userService = userService;
        _configuration = configuration;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto param)
    {
        if (await _userRepository.GetUserByEmail(param.Email) != null)
            return BadRequest("Email already inserted");

        if (await _userRepository.GetUserByUsername(param.Username) != null)
            return BadRequest("Username already inserted");

        var user = await _userService.InsertUserEncryptingPassword(
            new User()
            {
                Email = param.Email,
                UserName = param.Username
            }, param.Password);
        
        return Created(nameof(Register), new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            DateCreated = user.DateCreated
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto param)
    {
        var user = await _userRepository.GetUserByEmail(param.Email);
        
        if (user == null) return NotFound("Email does not exist");

        try
        {
            var jwt = await _userService.Login(user.Email, param.Password, user.PasswordSalt, 
                user.PasswordHash, _configuration.GetSection(Consts.JwtConfigSections).Value);
            return Ok(jwt);
        }
        catch (AuthenticationException e)
        {
            return Unauthorized(e.Message);
        }
    }

}