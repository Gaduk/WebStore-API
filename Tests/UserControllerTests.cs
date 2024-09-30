using Application.Features.User.Commands.CreateUser;
using Application.Features.User.Queries.GetUser;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Presentation.Controllers;

namespace Tests;

public class UserControllerTests
{
    private readonly UserController _userController;
    
    private readonly Mock<ILogger<UserController>> _logger = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly CancellationToken _cancellationToken;
    
    public UserControllerTests()
    {
        _userController = new UserController(
            _logger.Object,
            _mediatorMock.Object,
            _mapper.Object);
        
        _cancellationToken = _cancellationTokenSource.Token;
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
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserQuery>(), _cancellationToken))
            .ReturnsAsync(new User { UserName = input.UserName });
        
        // Act
        var result = await _userController.CreateUser(input, _cancellationToken);
        
        // Assert
        Assert.IsType<ConflictObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateUser_Returns400_WhenInputIsInvalid()
    {
        // Arrange
        var input = new CreateUserCommand(
            "user", 
            "1", //too short password
            "FirstName",
            "LastName",
            "+79990001122",
            "user@mail.ru"
        );
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserQuery>(), _cancellationToken))
            .ReturnsAsync((User?)null);
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), _cancellationToken))
            .ReturnsAsync((
                IdentityResult.Failed(), 
                new User { UserName = input.UserName }));
        
        // Act
        var result = await _userController.CreateUser(input, _cancellationToken);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateUser_Returns200_WhenInputLoginIsUnique_And_InputIsValid()
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
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserQuery>(), _cancellationToken))
            .ReturnsAsync((User?)null);
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), _cancellationToken))
            .ReturnsAsync((
                IdentityResult.Success, 
                new User { UserName = input.UserName }));
        
        // Act
        var result = await _userController.CreateUser(input, _cancellationToken);
        
        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }
}