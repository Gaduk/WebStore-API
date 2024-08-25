using System.Security.Claims;
using Application.Dto;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.UpdateUserRole;
using Application.Features.User.Queries.GetAllUsers;
using Application.Features.User.Queries.GetUserByLogin;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_API.Inputs;

namespace Web_API.Controllers;

[ApiController]
public class UserController(
    IMediator mediator, 
    SignInManager<User> signInManager, 
    UserManager<User> userManager, 
    IAuthorizationService authorizationService) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(RegisterInput input)
    {
        var userWithSameLogin = await mediator.Send(new GetUserByLoginQuery(input.Login));
        if (userWithSameLogin != null)
        {
            return Conflict("Пользователь с таким логином уже существует");
        }

        var user = new User
        {
            UserName = input.Login,
            FirstName = input.FirstName,
            LastName = input.LastName,
            PhoneNumber = input.PhoneNumber
        };
        
        var result = await userManager.CreateAsync(user, input.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, input.Login),
            new Claim(ClaimTypes.Role, "user")
        };
        await userManager.AddClaimsAsync(user, claims);
        
        return Ok($"Пользователь {user.UserName} успешно зарегистрирован");
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(string login, string password)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if (user is null) return Unauthorized("Неверное имя пользователя");

        var result = await signInManager.PasswordSignInAsync(user, password, false, false);
        if(!result.Succeeded) 
        {
            return Unauthorized("Неверный пароль");
        }
        return Ok($"Выполнен вход в аккаунт {login}");
    }
    
    [Authorize(Roles = "user, admin")]
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok("Выполнен выход из аккаунта");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("/{login}/makeAdmin")]
    public async Task<IActionResult> MakeAdmin(string login)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        await mediator.Send(new UpdateUserRoleCommand(user, true));
        var claim = new Claim(ClaimTypes.Role, "admin");
        await userManager.AddClaimAsync(user, claim);
        
        return Ok($"Пользователю {login} выданы права администратора");
    }
    
    [HttpGet("/accessDenied")]
    public async Task<IActionResult> DenyAccess()
    {
        return StatusCode(StatusCodes.Status403Forbidden);
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/user/{login}")]
    public async Task<IActionResult> DeleteUser(string login)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        await mediator.Send(new DeleteUserCommand(user));
        return Ok($"Пользователь {user.UserName} удален");
    }
    
    [Authorize("HaveAccess")]
    [HttpGet("/{login}")]
    public async Task<IActionResult> GetUser(string login)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if(user == null)
        {
            return NotFound("Пользователь не найден");
        }
        return Ok(user);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }
}