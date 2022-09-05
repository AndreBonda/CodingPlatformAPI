using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using CodingPlatform.Web.Global;

namespace CodingPlatform.Web.DTO;

public class UserDto
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public DateTime DateCreated { get; set; }
}

public class LoginUserDto
{
    [Required]
    [RegularExpression(Consts.EmailRegex, ErrorMessage = Consts.EmailFormatError)]
    public string Email { get; set; }
    [Required]
    [MinLength(6)]
    [RegularExpression(Consts.PasswordRegex, ErrorMessage = Consts.PasswordFormatError)]
    public string Password { get; set; }
}

public class RegisterUserDto : LoginUserDto
{
    [Required]
    [MinLength(3)]
    [RegularExpression(Consts.UsernameRegex, ErrorMessage = Consts.UsernameFormatError)]
    public string Username { get; set; }
}