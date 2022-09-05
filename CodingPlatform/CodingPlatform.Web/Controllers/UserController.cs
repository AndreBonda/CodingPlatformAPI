using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    
    public UserController(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto param)
    {
        if ((await _userRepository.GetFiltered(new UserFilters() {Email = param.Email})).Any())
            return BadRequest("Email already inserted");

        if ((await _userRepository.GetFiltered(new UserFilters() {Username = param.Username})).Any())
            return BadRequest(("Username already inserted"));

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
}