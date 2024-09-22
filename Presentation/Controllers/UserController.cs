using System.Security.Claims;
using Application.Features.User.Commands.CreateUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.UpdateUserRole;
using Application.Features.User.Queries.GetAllUsers;
using Application.Features.User.Queries.GetUser;
using Application.Services;
using Domain.Entities;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;


[ApiController]
public class UserController(
    ILogger<UserController> logger,
    IMediator mediator, 
    SignInManager<User> signInManager, 
    UserManager<User> userManager, 
    IAuthorizationService authorizationService,
    IMailService mailService) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        logger.LogInformation("HTTP POST /register");
        
        var userWithSameLogin = await userManager.FindByNameAsync(command.Login);
        if (userWithSameLogin != null)
        {
            logger.LogWarning("Conflict. User {login} already exist", command.Login);
            return Conflict($"User {command.Login} already exist");
        }

        var user = new User
        {
            UserName = command.Login,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email
        };
        
        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            logger.LogWarning("BadRequest\n{@errors}", result.Errors);
            return BadRequest(result.Errors);
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, command.Login),
            new(ClaimTypes.Role, "user")
        };
        await userManager.AddClaimsAsync(user, claims);
        
        RecurringJob.AddOrUpdate(
            $"SendEmailMinutelyTo_{command.Login}", 
            () => mailService.SendMessage(command.Email), 
            Cron.Minutely);
        
        logger.LogInformation("User {login} is signed up successfully", user.UserName);
        return Ok($"User {user.UserName} is signed up successfully");
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(string login, string password)
    {
        logger.LogInformation("HTTP POST /login");
        
        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            logger.LogWarning("Unauthorized. Wrong login");
            return Unauthorized("Wrong login");
        }
        
        var resultTask = signInManager.CheckPasswordSignInAsync(user, password, false);
        var claimsTask = userManager.GetClaimsAsync(user);
        await Task.WhenAll(resultTask, claimsTask);
        
        var result = resultTask.Result;
        var claims = claimsTask.Result;
        
        if(!result.Succeeded) 
        {
            logger.LogWarning("Unauthorized. Wrong password");
            return Unauthorized("Wrong password");
        }
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        logger.LogInformation("User {login} is signed in successfully", user.UserName);
        return Ok($"User {user.UserName} is signed in");
    }
    
    [Authorize(Roles = "user, admin")]
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout()
    {
        logger.LogInformation("HTTP GET /logout");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        var login = User.Identity?.Name;
        logger.LogInformation("User {login} is signed out", login);
        return Ok($"User {login} is signed out");
    }
    
    [HttpGet("/accessDenied")]
    public IActionResult DenyAccess()
    {
        logger.LogInformation("HTTP GET /accessDenied");
        return StatusCode(StatusCodes.Status403Forbidden);
    }
    
    [HttpGet("/users/{login}")]
    public async Task<IActionResult> GetUser(string login, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /users/{login}", login);
        
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
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
        
        var user = await userManager.FindByNameAsync(login);
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
    public async Task<IActionResult> UpdateUserStatus(string login, bool isAdmin, CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP PATCH /users/{login}", login);
        
        var user = await userManager.FindByNameAsync(login);
        
        if (user == null)
        {
            logger.LogWarning("NotFound. User {login} is not found", login);
            return NotFound($"User {login} is not found");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, "admin")
        };
        switch (isAdmin)
        {
            case true when !user.IsAdmin:
            {
                await userManager.AddClaimsAsync(user, claims);
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                User.AddIdentity(claimsIdentity);
                break;
            }
            case false when user.IsAdmin:
                await userManager.RemoveClaimsAsync(user, claims);
                break;
        }

        await mediator.Send(new UpdateUserRoleCommand(user, isAdmin), cancellationToken);
        
        logger.LogInformation("{login} rights is updated", user.UserName);
        return Ok($"{user.UserName} rights is updated");
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        logger.LogInformation("HTTP GET /users");
        
        var users = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(users);
    }
}