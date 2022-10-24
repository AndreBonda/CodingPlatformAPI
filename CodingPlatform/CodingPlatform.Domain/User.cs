using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain;

public class User : BaseEntity
{
    [Required]
    public string Email { get; protected set; }
    [Required]
    public string Username { get; protected set; }
    [Required]
    public byte[] PasswordSalt { get; protected set; }
    [Required]
    public byte[] PasswordHash { get; protected set; }

    private User() { }

    public static User CreateNew(string email, string username, byte[] passwordSalt, byte[] passwordHash)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

        if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

        if (passwordSalt == null) throw new ArgumentNullException(nameof(passwordSalt));

        if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

        return new User()
        {
            Email = email,
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
    }
}