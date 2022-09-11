using System.Security.Authentication;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.AppCore.Services;
using CodingPlatform.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace CodingPlatform.AppCore.UnitTests;

[TestFixture]
public class UserServiceTests
{
    private IUserService _userService;
    private Mock<IUserRepository> _userRepository;

    [SetUp]
    public void SetUp()
    {
        _userRepository = new Mock<IUserRepository>();
        _userService = new UserService(_userRepository.Object);
    }

    [Test]
    public async Task InsertUserEncryptingPassword_WhenCalled_SetPasswordHashAndSalt()
    {
        var user = new User();

        await _userService.InsertUserEncryptingPassword(user, "password");

        Assert.That(user.PasswordHash, Is.Not.Null);
        Assert.That(user.PasswordHash.Length, Is.GreaterThan(1));
        Assert.That(user.PasswordSalt, Is.Not.Null);
        Assert.That(user.PasswordSalt.Length, Is.GreaterThan(1));
    }

    [Test]
    public async Task InsertUserEncryptingPassword_WhenCalled_StoreTheUser()
    {
        var user = new User();

        await _userService.InsertUserEncryptingPassword(user, "password");

        _userRepository.Verify(userRepo => userRepo.InsertAsync(user));
    }
    
    [TestCase("", "password", new byte[0], new byte[0], "keygen", "email")]
    [TestCase(" ", "password", new byte[0], new byte[0], "keygen", "email")]
    [TestCase("email", "", new byte[0], new byte[0], "keygen", "password")]
    [TestCase("email", "", new byte[0], new byte[0], "keygen", "password")]
    [TestCase("email", "password", null, new byte[0], "keygen", "salt")]
    [TestCase("email", "password", null, new byte[0], "keygen", "salt")]
    [TestCase("email", "password", new byte[0], null, "keygen", "password")]
    [TestCase("email", "password", new byte[0], null, "keygen", "password")]
    public void Login_MissingInputArguments_ThrowArgumentNullException(string email, string password, byte[] salt,
        byte[] hashPw, string keyGen, string excMsgExpected)
    {
        var exc = Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.Login(email, password, salt, hashPw, keyGen));
        Assert.That(exc.Message, Does.Contain(excMsgExpected).IgnoreCase);
    }

    [Test]
    public void Login_WrongPassword_ThrowAuthenticationException()
    {
        var expectedException = Assert.ThrowsAsync<AuthenticationException>(
            async () => await _userService.Login("email", "password", Array.Empty<byte>(), Array.Empty<byte>(), "keygen"));

        Assert.That(expectedException.Message, Does.Contain("password").IgnoreCase);
    }

    [Ignore("Setup problem HMACSHA512")]
    public void Login_CorrectPassword_ReturnJWT()
    {
        string plainTextPassword = "password";
        byte[] salt = new byte[]
        {
            96, 20, 253, 85, 192, 82, 108, 148, 2, 29, 133, 73, 67, 197, 53, 65, 247, 105, 129, 204, 209, 219, 106, 166,
            184, 130, 131, 162, 206, 228, 56, 69, 201, 70, 76, 101, 47, 20, 23, 101, 57, 230, 14, 24, 196, 239, 104,
            107, 207, 224, 132, 114, 55, 40, 81, 254, 44, 6, 154, 208, 82, 91, 208, 128, 54, 179, 105, 154, 172, 27,
            220, 42, 147, 19, 84, 98, 237, 34, 67, 185, 206, 28, 22, 21, 200, 170, 195, 230, 143, 179, 14, 26, 29, 167,
            60, 251, 215, 174, 144, 72, 229, 152, 191, 229, 122, 116, 188, 60, 184, 139, 148, 72, 134, 48, 83, 101, 45,
            200, 136, 93, 244, 252, 183, 145, 16, 38, 247, 245
        };
        byte[] hashPassword = new byte[]
        {
            123,36,18,234,102,123,15,89,67,76,50,50,92,213,199,149,160,130,192,223,212,172,89,121,93,195,30,186,115,178,176,214,152,246,75,210,43,250,233,236,1,219,50,80,159,44,56,125,217,214,166,205,91,8,200,221,176,35,208,75,94,104,88,157
        };

        var result = _userService.Login("email", plainTextPassword, salt, hashPassword, "keygen");
    }
}