using Application.Features.User.Commands.CreateUser;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web_API.Controllers;

namespace Tests;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IAuthorizationService> _authorizationServiceMock = new();
    private readonly Mock<IMailService> _mailServiceMock = new();
    private readonly UserController _userController;
    
    public UserControllerTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), 
            null!, null!, null!, null!, null!, null!, null!, null!);
        
        Mock<SignInManager<User>> signInManagerMock = new(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(), 
            null!, null!, null!, null!);
        
        _userController = new UserController(
            _mediatorMock.Object , 
            signInManagerMock.Object,
            _userManagerMock.Object,
            _authorizationServiceMock.Object,
            _mailServiceMock.Object);
    }
    
    [Fact]
    public async Task CreateUser_Returns409_WhenInputLoginIsNotUnique()
    {
        // Arrange
        var input = new CreateUserCommand(
            "user", 
            "user", 
            "FirstName",
            "LastName",
            "+79990001122",
            "user@mail.ru"
        );
        _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                UserName = "user",
                FirstName = "",
                LastName = "",
                PhoneNumber = "",
                Email = ""
            });
        
        // Act
        var result = await _userController.CreateUser(input);
        
        // Assert
        Assert.IsType<ConflictObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateUser_Returns400_WhenInputIsInvalid()
    {
        // Arrange
        var input = new CreateUserCommand(
            "user2", 
            "1", //too short password
            "FirstName",
            "LastName",
            "+79990001122",
            "user@mail.ru"
        );
        
        _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());
        
        // Act
        var result = await _userController.CreateUser(input);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}