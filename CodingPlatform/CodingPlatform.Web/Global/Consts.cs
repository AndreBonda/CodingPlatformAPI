namespace CodingPlatform.Web.Global;

public static class Consts
{
    public const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    public const string EmailFormatError =
        @"Email format not valid";
    public const string PasswordRegex = @"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$";
    public const string PasswordFormatError =
        "Password must contain an uppercase letter, a lowercase letter, a number and a special character";
    public const string UsernameRegex = @"^[A-Za-z0-9]*$";
    public const string UsernameFormatError = @"Username must be an alphanumeric";
    public const string JwtConfigSections = "JWT:Key";
}