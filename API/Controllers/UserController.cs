using System.Security.Claims;
using Application.Dto.User;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.UpdateUserRole;
using Application.Features.User.Queries.GetAllUsers;
using Application.Features.User.Queries.GetUser;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers;


[ApiController]
public class UserController(
    IMediator mediator, 
    SignInManager<User> signInManager, 
    UserManager<User> userManager, 
    IAuthorizationService authorizationService,
    IMailService mailService) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(CreateUserDto input)
    {
        var userWithSameLogin = await userManager.FindByNameAsync(input.Login);
        if (userWithSameLogin != null)
        {
            return Conflict("Пользователь с таким логином уже существует");
        }

        var user = new User
        {
            UserName = input.Login,
            FirstName = input.FirstName,
            LastName = input.LastName,
            PhoneNumber = input.PhoneNumber,
            Email = input.Email
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
        
        //Подключаем рассылку
        mailService.SendMessage(input.Login, input.Email);
        
        return Ok($"Пользователь {user.UserName} успешно зарегистрирован");
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(string login, string password)
    {
        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return Unauthorized("Неверное имя пользователя");
        }
        
        var resultTask = signInManager.CheckPasswordSignInAsync(user, password, false);
        var claimsTask = userManager.GetClaimsAsync(user);
        await Task.WhenAll(resultTask, claimsTask);
        
        var result = resultTask.Result;
        var claims = claimsTask.Result;

        
        if(!result.Succeeded) 
        {
            return Unauthorized("Неверный пароль");
        }
        
        foreach (var claim in claims)
        {
            Console.WriteLine(claim.Value);
        }
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        
        return Ok($"Выполнен вход в аккаунт {login}");
    }
    
    [Authorize(Roles = "user, admin")]
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("Выполнен выход из аккаунта");
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("/user/{login}/isAdmin")]
    public async Task<IActionResult> MakeAdmin(string login)
    {
        var user = await userManager.FindByNameAsync(login);
        
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        
        if (user.IsAdmin)
        {
            return Conflict("Пользователь уже является администратором");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "admin")
        };
        await userManager.AddClaimsAsync(user, claims);
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        User.AddIdentity(claimsIdentity);
        
        await mediator.Send(new UpdateUserRoleCommand(user, true));
        
        return Ok($"Пользователю {login} выданы права администратора");
    }
    
    [HttpGet("/accessDenied")]
    public IActionResult DenyAccess()
    {
        return StatusCode(StatusCodes.Status403Forbidden);
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/user/{login}")]
    public async Task<IActionResult> DeleteUser(string login)
    {
        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        await mediator.Send(new DeleteUserCommand(user));
        return Ok($"Пользователь {user.UserName} удален");
    }
    
    [HttpGet("user/{login}")]
    public async Task<IActionResult> GetUser(string login)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var user = await mediator.Send(new GetUserQuery(login));
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