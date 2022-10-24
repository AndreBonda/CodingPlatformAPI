using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IUserService
{
    Task<User> Register(string email, string username, string plainTextPassword);
    /// <summary>
    /// Login the user.
    /// </summary>
    /// <returns>Returns a jwt or an empty string if authentication failed</returns>
    Task<string> Login(string email, string plainTextPassword, string keyGen);
}