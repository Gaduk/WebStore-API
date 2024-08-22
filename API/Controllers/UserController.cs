using System.Security.Claims;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Queries.GetAllUsers;
using Application.Features.User.Queries.GetUserByLogin;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_API.Inputs;

namespace Web_API.Controllers;

[ApiController]
public class UserController(
    IMediator mediator, 
    SignInManager<User> signInManager, 
    UserManager<User> userManager) : ControllerBase
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
            new Claim("Name", input.Login),
            new Claim("Role", "User")
        };
        await userManager.AddClaimsAsync(user, claims);
        
        return Ok($"Пользователь {user.UserName} успешно зарегистрирован");
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(string login, string password)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if (user is null) return Unauthorized();

        await signInManager.SignInAsync(user, true);
        return Ok($"Выполнен вход в аккаунт {login}");
    }
    
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok("Выполнен выход из аккаунта");
    }
    
    [HttpGet("/{login}/makeAdmin")]
    public async Task<IActionResult> MakeAdmin(string login)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        //var identity = (ClaimsIdentity)User.Identity!;
        //var existingClaim = identity.FindFirst(ClaimTypes.Role);
        //if (existingClaim != null)
        //{
        //    identity.RemoveClaim(existingClaim);
        //}
        var claim = new Claim("Role", "Admin");
        await userManager.AddClaimAsync(user, claim);
        return Ok($"Пользователю {login} выданы права администратора");
    }
    
    [HttpGet("/accessDenied")]
    public async Task<IActionResult> DenyAccess()
    {
        return Forbid("Доступ запрещен");
    }
    
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
    
    [HttpGet("/{login}")]
    public async Task<IActionResult> GetUser(string login)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(login));
        if(user == null)
        {
            return NotFound("Пользователь не найден");
        }
        return Ok(user);
    }
    
    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }
}