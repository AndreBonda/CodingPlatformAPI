namespace CodingPlatform.Domain.Interfaces.Utility;

public interface IAuthenticationProvider
{
    (byte[] Key, byte[] ComputedHash) HashPassword(string plainText);
    bool VerifyPassword(string plainTextPassword, byte[] salt, byte[] hashPassword);
    string GenerateJWT(long userId, string email, string keyGen);
}

