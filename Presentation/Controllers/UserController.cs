using Application.Features.User.Commands.CreateUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.Login;
using Application.Features.User.Commands.Logout;
using Application.Features.User.Commands.UpdateUserRole;
using Application.Features.User.Queries.GetUser;
using Application.Features.User.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IMediator mediator) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        
        logger.LogInformation("User {username} is signed up successfully", command.UserName);
        return CreatedAtAction(
            nameof(GetUser),
            new { username = command.UserName },
            null
        );
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(string username, string password, CancellationToken cancellationToken)
    {
        await mediator.Send(new LoginCommand(username, password), cancellationToken);
        
        logger.LogInformation("User {username} is signed in successfully", username);
        return Ok($"User {username} is signed in");
    }
    
    [Authorize(Roles = "user")]
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await mediator.Send(new LogoutCommand(), cancellationToken);
        
        var username = User.Identity?.Name;
        logger.LogInformation("User {username} is signed out", username);
        return Ok($"User {username} is signed out");
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(users);
    }
    
    [Authorize(Roles = "user")]
    [HttpGet("/users/{username}")]
    public async Task<IActionResult> GetUser(string username, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery(username), cancellationToken);
        return Ok(user);
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/users/{username}")]
    public async Task<IActionResult> DeleteUser(string username, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteUserCommand(username), cancellationToken);
        
        logger.LogInformation("User {username} is deleted", username);
        return Ok($"User {username} is deleted");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPatch("/users/{username}")]
    public async Task<IActionResult> UpdateUserRole(string username, bool isAdmin, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateUserRoleCommand(username, isAdmin), cancellationToken);
        
        logger.LogInformation("{username} rights is updated", username);
        return Ok($"{username} rights is updated");
    }
}