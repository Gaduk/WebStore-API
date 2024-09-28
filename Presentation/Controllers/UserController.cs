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
using Domain.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;

[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IMediator mediator) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /register");
        
        var userWithSameLogin = await mediator.Send(new GetUserQuery(command.Login), cancellationToken);
        if (userWithSameLogin != null)
        {
            logger.LogWarning("Conflict. User {login} already exist", command.Login);
            return Conflict($"User {command.Login} already exist");
        }

        var (result, user) = await mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
        {
            logger.LogWarning("BadRequest\n{@errors}", result.Errors);
            return BadRequest(result.Errors);
        }
        await mediator.Send(new AddUserClaims(user), CancellationToken.None);
        
        await mediator.Send(new SubscribeUserToMailingCommand(command.Email, command.Login), CancellationToken.None);
        
        logger.LogInformation("User {login} is signed up successfully", command.Login);
        return CreatedAtAction(
            nameof(GetUser),
            new { login = command.Login },
            new UserDto(
                user.UserName, 
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Email,
                user.IsAdmin
                )
        );
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(string login, string password, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP POST /login");
        
        var user = await mediator.Send(new GetUserQuery(login), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("Unauthorized. Wrong login");
            return Unauthorized("Wrong login");
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

        logger.LogInformation("User {login} is signed in successfully", user.UserName);
        return Ok($"User {user.UserName} is signed in");
    }
    
    [Authorize(Roles = "user, admin")]
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /logout");

        await mediator.Send(new SignOutCommand(HttpContext), cancellationToken);
        
        var login = User.Identity?.Name;
        logger.LogInformation("User {login} is signed out", login);
        return Ok($"User {login} is signed out");
    }
    
    [HttpGet("/users/{login}")]
    public async Task<IActionResult> GetUser(string login, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /users/{login}", login);
        
        var authorizationResult = await mediator.Send(
            new CheckAccessToResourceQuery(User, login, "HaveAccess"), cancellationToken);
        if (!authorizationResult.Succeeded)
        {
            logger.LogWarning("Forbidden. No access");
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var user = await mediator.Send(new GetUserQuery(login), cancellationToken);
        if(user == null)
        {
            logger.LogWarning("NotFound. User {login} is not found", login);
            return NotFound($"User {login} is not found");
        }
        return Ok(user);
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/users/{login}")]
    public async Task<IActionResult> DeleteUser(string login, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP DELETE /users/{login}", login);
        
        var user = await mediator.Send(new GetUserQuery(login), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("NotFound. User {login} is not found", login);
            return NotFound($"User {login} is not found");
        }
        await mediator.Send(new DeleteUserCommand(user), cancellationToken);
        
        logger.LogInformation("User {login} is deleted", user.UserName);
        return Ok($"User {user.UserName} is deleted");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPatch("/users/{login}")]
    public async Task<IActionResult> UpdateUserRole(string login, bool isAdmin, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /users/{login}", login);
        
        var user = await mediator.Send(new GetUserQuery(login), cancellationToken);
        if (user == null)
        {
            logger.LogWarning("NotFound. User {login} is not found", login);
            return NotFound($"User {login} is not found");
        }
        
        await mediator.Send(new UpdateUserRoleCommand(user, isAdmin), cancellationToken);
        await mediator.Send(new AddAdminClaimsCommand(user, isAdmin), CancellationToken.None);
        
        logger.LogInformation("{login} rights is updated", user.UserName);
        return Ok($"{user.UserName} rights is updated");
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /users");
        var users = await mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(users);
    }
}