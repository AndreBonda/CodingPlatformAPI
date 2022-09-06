using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IUserService
{
    Task<User> InsertUserEncryptingPassword(User user, string plainTextPassword);
    /// <summary>
    /// Login the user generating a jwt
    /// </summary>
    /// <param name="email"></param>
    /// <param name="plainTextPassword"></param>
    /// <param name="hashPassword"></param>
    /// <param name="keyGen"></param>
    /// <returns>Return JWT</returns>
    Task<string> Login(string email, string plainTextPassword, byte[] salt, byte[] hashPassword, string keyGen);
}