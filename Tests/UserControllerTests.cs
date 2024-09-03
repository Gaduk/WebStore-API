using Application.Features.User.Queries.GetUserByLogin;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web_API.Controllers;
using Web_API.Inputs;

namespace Tests;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IAuthorizationService> _authorizationServiceMock = new();
    private readonly Mock<IMailService> _mailServiceMock = new();
    private readonly UserController _userController;
    
    public UserControllerTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), 
            null, null, null, null, null, null, null, null);
        
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(), 
            null, null, null, null);
        
        _userController = new UserController(
            _mediatorMock.Object , 
            _signInManagerMock.Object,
            _userManagerMock.Object,
            _authorizationServiceMock.Object,
            _mailServiceMock.Object);
    }
    
    [Fact]
    public async Task CreateUser_Returns409_WhenInputLoginIsNotUnique()
    {
        // Arrange
        var input = new RegisterInput(
            "user", 
            "user", 
            "FirstName",
            "LastName",
            "+79990001122",
            "user@mail.ru"
        );
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByLoginQuery>(), default))
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
        var input = new RegisterInput(
            "user2", 
            "1", //too short password
            "FirstName",
            "LastName",
            "+79990001122",
            "user@mail.ru"
        );
        
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());
        
        // Act
        var result = await _userController.CreateUser(input);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateUser_Returns200_WhenInputLoginIsUnique_And_InputIsValid()
    {
        // Arrange
        var input = new RegisterInput(
            "user2", 
            "user", 
            "FirstName",
            "LastName",
            "+79990001122",
            "user@mail.ru"
        );
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByLoginQuery>(), default))
            .ReturnsAsync((User?)null);
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        var result = await _userController.CreateUser(input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}