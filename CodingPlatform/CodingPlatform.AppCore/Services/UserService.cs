using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain;
using Microsoft.IdentityModel.Tokens;

namespace CodingPlatform.AppCore.Services;

public class UserService : IUserService
{
    //TODO: a lot of coupling inside. Refactor. Methods do a lot of things.
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> InsertUserEncryptingPassword(string email, string username, string plainTextPassword)
    {
        var passwordHash = CreatePasswordHash(plainTextPassword);
        var user = new User(email, username, passwordHash.PasswordSalt, passwordHash.PasswordHash);
        return await _userRepository.InsertAsync(user);
    }

    public async Task<string> Login(string email, string plainTextPassword, byte[] salt, byte[] hashPassword, string keyGen)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email required");
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw new ArgumentNullException("plain text password required");
        if (hashPassword == null)
            throw new ArgumentNullException("hash password required");
        if (salt == null)
            throw new ArgumentNullException("salt password required");

        if (!VerifyPassword(plainTextPassword, salt, hashPassword))
            throw new AuthenticationException("wrong password");

        var user = await _userRepository.GetUserByEmail(email);

        return CreateJwt(user.Id, email, keyGen);
    }

    private (byte[] PasswordSalt, byte[] PasswordHash) CreatePasswordHash(string plainTextPassword)
    {
        using var hmac = new HMACSHA512();
        return new(
            hmac.Key,
            hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainTextPassword)));
    }

    private bool VerifyPassword(string plainTextPassword, byte[] salt, byte[] hashPassword)
    {
        using var hmac = new HMACSHA512(salt);
        return hashPassword.SequenceEqual(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainTextPassword)));
    }

    private string CreateJwt(long userId, string email, string keyGen)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyGen));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: cred);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}