using Application.Dto.User;
using Application.Features.User.Commands.AddAdminClaims;
using Application.Features.User.Commands.AddUserClaims;
using Application.Features.User.Commands.CreateUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.SignIn;
using Application.Features.User.Commands.SignOut;
using Application.Features.User.Commands.SubscribeUserToMailing;
using Application.Features.User.Commands.UpdateUserRole;
using Application.Features.User.Queries.CheckAccessToResource;
using Application.Features.User.Queries.CheckPassword;
using Application.Features.User.Queries.GetUser;
using Application.Features.User.Queries.GetUserClaims;
using Application.Features.User.Queries.GetUsers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IMediator mediator,
    IMapper mapper) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /register");
        
        var userWithSameName = await mediator.Send(new GetUserQuery(command.UserName), cancellationToken);
        if (userWithSameName != null)
        {
            logger.LogWarning("Conflict. User {username} already exist", command.UserName);
            return Conflict($"User {command.UserName} already exist");
        }

        var (result, user) = await mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
        {
            logger.LogWarning("BadRequest\n{@errors}", result.Errors);
            return BadRequest(result.Errors);
        }
        await mediator.Send(new AddUserClaims(user), CancellationToken.None);
        
        await mediator.Send(new SubscribeUserToMailingCommand(command.Email, command.UserName), CancellationToken.None);
        
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
        logger.LogInformation("HTTP POST /login");
        
        var user = await mediator.Send(new GetUserQuery(username), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("Unauthorized. Wrong username");
            return Unauthorized("Wrong username");
        }

        var checkPasswordTask = mediator.Send(new CheckPasswordQuery(user, password), cancellationToken);
        var getUserClaimsTask = mediator.Send(new GetUserClaimsQuery(user), cancellationToken);
        await Task.WhenAll(checkPasswordTask, getUserClaimsTask);
        
        var passwordIsRight = checkPasswordTask.Result;
        var claims = getUserClaimsTask.Result;
        
        if(!passwordIsRight) 
        {
            logger.LogWarning("Unauthorized. Wrong password");
            return Unauthorized("Wrong password");
        }
        
        await mediator.Send(new SignOutCommand(HttpContext), cancellationToken);
        await mediator.Send(new SignInCommand(claims, HttpContext), CancellationToken.None);

        logger.LogInformation("User {username} is signed in successfully", user.UserName);
        return Ok($"User {user.UserName} is signed in");
    }
    
    [Authorize(Roles = "user")]
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /logout");

        await mediator.Send(new SignOutCommand(HttpContext), cancellationToken);
        
        var username = User.Identity?.Name;
        logger.LogInformation("User {username} is signed out", username);
        return Ok($"User {username} is signed out");
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /users");
        
        var users = await mediator.Send(new GetUsersQuery(), cancellationToken);
        
        var usersDto = mapper.Map<List<UserDto>>(users);
        return Ok(usersDto);
    }
    
    [Authorize(Roles = "user")]
    [HttpGet("/users/{username}")]
    public async Task<IActionResult> GetUser(string username, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /users/{login}", username);
        
        var authorizationResult = await mediator.Send(
            new CheckAccessToResourceQuery(User, username, "HaveAccess"), cancellationToken);
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var user = await mediator.Send(new GetUserQuery(username), cancellationToken);
        if(user == null)
        {
            logger.LogWarning("NotFound. User {username} is not found", username);
            return NotFound($"User {username} is not found");
        }

        var userDto = mapper.Map<UserDto>(user);
        return Ok(userDto);
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/users/{username}")]
    public async Task<IActionResult> DeleteUser(string username, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP DELETE /users/{username}", username);
        
        var user = await mediator.Send(new GetUserQuery(username), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("NotFound. User {username} is not found", username);
            return NotFound($"User {username} is not found");
        }
        await mediator.Send(new DeleteUserCommand(user), cancellationToken);
        
        logger.LogInformation("User {username} is deleted", user.UserName);
        return Ok($"User {user.UserName} is deleted");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPatch("/users/{username}")]
    public async Task<IActionResult> UpdateUserRole(string username, bool isAdmin, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /users/{login}", username);
        
        var user = await mediator.Send(new GetUserQuery(username), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("NotFound. User {username} is not found", username);
            return NotFound($"User {username} is not found");
        }
        
        await mediator.Send(new UpdateUserRoleCommand(user, isAdmin), cancellationToken);
        await mediator.Send(new AddAdminClaimsCommand(user, isAdmin), CancellationToken.None);
        
        logger.LogInformation("{username} rights is updated", user.UserName);
        return Ok($"{user.UserName} rights is updated");
    }
}