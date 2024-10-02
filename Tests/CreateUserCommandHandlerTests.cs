using Application.Exceptions;
using Application.Features.User.Commands.CreateUser;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests;

public class CreateUserCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CreateUserCommandHandler _handler;
        private readonly CreateUserCommand _command;

        public CreateUserCommandHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), 
                null!, null!, null!, null!, null!, null!, null!, null!);
            _userRepositoryMock = new Mock<IUserRepository>();
            Mock<IMailService> mailServiceMock = new();
            
            _handler = new CreateUserCommandHandler(
                _userManagerMock.Object,
                _userRepositoryMock.Object,
                mailServiceMock.Object);
            
            _command = new CreateUserCommand(
                "user", 
                "user", 
                "FirstName",
                "LastName",
                "+79990001122",
                "user@mail.ru"
            );
        }

        [Fact]
        public async Task Handle_ThrowConflictException_WhenUserAlreadyExists()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUser(
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User());

            // Act
            var exception = await Record.ExceptionAsync(() => _handler.Handle(_command, CancellationToken.None));
            
            //Assert
            Assert.IsType<ConflictException>(exception);
        }

        [Fact]
        public async Task Handle_ThrowBadRequestException_WhenCreatingFails()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUser(
                    It.IsAny<string>(), 
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            _userManagerMock.Setup(um => um.CreateAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var exception = await Record.ExceptionAsync(() => _handler.Handle(_command, CancellationToken.None));
            
            //Assert
            Assert.IsType<BadRequestException>(exception);
        }
        
        [Fact]
        public async Task Handle_ThrowNoExceptions_WhenCreatingSucceeded()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUser(
                    It.IsAny<string>(), 
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            _userManagerMock.Setup(um => um.CreateAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var exception = await Record.ExceptionAsync(() => _handler.Handle(_command, CancellationToken.None));
            
            // Assert
            Assert.Null(exception);
        }
    }