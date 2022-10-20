using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Services;

public interface IUserService
{
    Task<User> InsertUserEncryptingPassword(string email, string username, string plainTextPassword);
    Task<string> Login(string email, string plainTextPassword, byte[] salt, byte[] hashPassword, string keyGen);
}