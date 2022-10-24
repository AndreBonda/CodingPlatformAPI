using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using CodingPlatform.Domain.Exception;
using CodingPlatform.Domain.Interfaces.Utility;

namespace CodingPlatform.AppCore.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IAuthenticationProvider _authProvider;


    public UserService(IUserRepository userRepo, IAuthenticationProvider authProvider)
    {
        _userRepo = userRepo;
        _authProvider = authProvider;
    }

    public async Task<User> Register(string email, string username, string plainTextPassword)
    {
        if (await _userRepo.ExistUserByEmailAsync(email)) throw new BadRequestException("Email already inserted.");

        if (await _userRepo.ExistUserByUsernameAsync(username)) throw new BadRequestException("Username already inserted.");

        var password = _authProvider.HashPassword(plainTextPassword);
        var user = User.CreateNew(email, username, password.Key, password.ComputedHash);

        return await _userRepo.InsertAsync(user);
    }

    public async Task<string> Login(string email, string plainTextPassword, string keyGen)
    {
        var user = await _userRepo.GetUserByEmailAsync(email);

        if (user == null) throw new NotFoundException("User not found.");

        if (_authProvider.VerifyPassword(plainTextPassword, user.PasswordSalt, user.PasswordHash))
            return _authProvider.GenerateJWT(user.Id, user.Email, keyGen);

        return String.Empty;
    }
}