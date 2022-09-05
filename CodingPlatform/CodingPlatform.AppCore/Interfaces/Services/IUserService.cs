using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IUserService
{
    Task<User> InsertUserEncryptingPassword(User user, string plainTextPassword);
    /// <summary>
    /// Check the password provided and generate a JWT
    /// </summary>
    /// <param name="email"></param>
    /// <param name="plainTextPassword"></param>
    /// <param name="hashPassword"></param>
    /// <param name="keyGen"></param>
    /// <returns>Return JWT</returns>
    string Login(string email, string plainTextPassword, byte[] salt, byte[] hashPassword, string keyGen);
}