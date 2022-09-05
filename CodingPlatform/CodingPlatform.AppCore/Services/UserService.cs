using System.Security.Cryptography;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> InsertUserEncryptingPassword(User user, string plainTextPassword)
    {
        user.PasswordHash = CreatePasswordHash(plainTextPassword);
        return await _userRepository.InsertAsync(user);
    }

    private byte[] CreatePasswordHash(string plainTextPassword)
    {
        using var hmac = new HMACSHA512();
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainTextPassword));

    }
}