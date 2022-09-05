using System.ComponentModel.DataAnnotations;

namespace CodingPlatform.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
}