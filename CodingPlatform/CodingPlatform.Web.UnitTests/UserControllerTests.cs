using System.Security.Authentication;
using CodingPlatform.AppCore.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using CodingPlatform.AppCore.Interfaces.Services;
using CodingPlatform.Domain.Entities;
using CodingPlatform.Web.Controllers;
using CodingPlatform.Web.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CodingPlatform.Web.UnitTests;

[TestFixture]
public class UserControllerTests
{
    private UserController _userController;
    private Mock<IUserRepository> _userRepository;
    private Mock<IUserService> _userService;
    private Mock<IConfiguration> _configuration;
    private Mock<IConfigurationSection> _configurationSection;
    private Mock<IHttpContextAccessor> _httpCtxAccessor;
    
    [SetUp]
    public void SetUp()
    {
        _userRepository = new Mock<IUserRepository>();
        _userService = new Mock<IUserService>();
        _configuration = new Mock<IConfiguration>();
        _configurationSection = new Mock<IConfigurationSection>();
        _httpCtxAccessor = new Mock<IHttpContextAccessor>();
        _userController = new UserController(_userRepository.Object, _userService.Object, _configuration.Object,
            _httpCtxAccessor.Object);
        
        _configurationSection.Setup(s => s.Value).Returns("a");
        _configuration
            .Setup(c => c.GetSection(It.IsAny<string>()))
            .Returns(new Mock<IConfigurationSection>().Object);
        _configuration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(_configurationSection.Object);
    }

    [Test]
    public async Task Register_EmailAlreadyInserted_ReturnBadRequestObjectResult()
    {
        _userRepository
            .Setup(userRepo => userRepo.GetUserByEmail(It.IsAny<string>()))
            .Returns(Task.FromResult(new User()));
        
        var result = await _userController.Register(new RegisterUserDto());
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Register_UsernameAlreadyInserted_ReturnBadRequestObjectResult()
    {
        _userRepository
            .Setup(userRepo => userRepo.GetUserByUsername(It.IsAny<string>()))
            .Returns(Task.FromResult(new User()));
        
        var result = await _userController.Register(new RegisterUserDto());
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Register_ExpectedCredentials_ReturnCreatedResult()
    {
        var credential = new RegisterUserDto() {Email = "email", Username = "username", Password = "password"};
        _userRepository
            .Setup(userRepo => userRepo.GetUserByEmail(It.IsAny<string>()))
            .Returns(Task.FromResult((User)null));
        _userRepository
            .Setup(userRepo => userRepo.GetUserByUsername(It.IsAny<string>()))
            .Returns(Task.FromResult((User)null));
        _userService
            .Setup(userSvc => userSvc.InsertUserEncryptingPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult<User>(new User()
            {
                Id = 1,
                Email = credential.Email,
                UserName = credential.Username,
                DateCreated = new DateTime(2000, 1, 1)
            }));

        var result = await _userController.Register(credential);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<CreatedResult>());
    }

    [Test]
    public async Task Login_EmailDoesNotExist_ReturnBadRequestObjectResult()
    {
        _userRepository
            .Setup(userRepo => userRepo.GetUserByEmail(It.IsAny<string>()))
            .Returns(Task.FromResult((User)null));

        var result = await _userController.Login(new LoginUserDto());
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Login_WrongPassword_ReturnUnauthorizedObjectResult()
    {
        _userRepository
            .Setup(userRepo => userRepo.GetUserByEmail(It.IsAny<string>()))
            .Returns(Task.FromResult(new User()));
        _userService
            .Setup(userSvc => 
                userSvc.Login(It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<byte[]>(), 
                    It.IsAny<byte[]>(), 
                    It.IsAny<string>()))
            .Throws<AuthenticationException>();

        var result = await _userController.Login(new LoginUserDto());
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
    }

    [Test]
    public async Task Login_CorrectPassword_ReturnOkObjectResult()
    {
        _userRepository
            .Setup(userRepo => userRepo.GetUserByEmail(It.IsAny<string>()))
            .Returns(Task.FromResult(new User()));
        _userService
            .Setup(userSvc =>
                userSvc.Login(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<string>()))
            .Returns(Task.FromResult(It.IsAny<string>()));

        var result = await _userController.Login(new LoginUserDto());
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }
}