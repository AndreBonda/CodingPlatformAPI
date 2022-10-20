using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public class User : BaseEntity
{
    [Required]
    public string Email { get; private set; }
    [Required]
    public string Username { get; private set; }
    [Required]
    public byte[] PasswordSalt { get; private set; }
    [Required]
    public byte[] PasswordHash { get; private set; }

    private User() { }

    public User(string email, string username, byte[] passwordSalt, byte[] passwordHash)
    {
        Email = email;
        Username = username;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
    }
}