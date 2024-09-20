using System.Security.Claims;
using Application.Features.Order.Commands.CreateOrder;
using Application.Features.Order.Queries.GetUserOrders;
using Application.Features.OrderedGood.CreateOrderedGoods;
using Application.Features.User.Commands.CreateUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.UpdateUserRole;
using Application.Features.User.Queries.GetAllUsers;
using Application.Features.User.Queries.GetUser;
using Application.Services;
using Domain.Dto.OrderedGoods;
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
    IMediator mediator, 
    SignInManager<User> signInManager, 
    UserManager<User> userManager, 
    IAuthorizationService authorizationService,
    IMailService mailService) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var userWithSameLogin = await userManager.FindByNameAsync(command.Login);
        if (userWithSameLogin != null)
        {
            return Conflict("Пользователь с таким логином уже существует");
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
            return BadRequest(result.Errors);
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, command.Login),
            new(ClaimTypes.Role, "user")
        };
        await userManager.AddClaimsAsync(user, claims);
        
        //Подключаем рассылку
        RecurringJob.AddOrUpdate(
            $"SendEmailMinutelyTo_{command.Login}", 
            () => mailService.SendMessage(command.Email), 
            Cron.Minutely);
        
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
    
    [HttpGet("/accessDenied")]
    public IActionResult DenyAccess()
    {
        return StatusCode(StatusCodes.Status403Forbidden);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("/users/{login}/adminStatus")]
    public async Task<IActionResult> MakeAdmin(string login, CancellationToken cancellationToken)
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
            new(ClaimTypes.Role, "admin")
        };
        await userManager.AddClaimsAsync(user, claims);
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        User.AddIdentity(claimsIdentity);
        
        await mediator.Send(new UpdateUserRoleCommand(user, true), cancellationToken);
        
        return Ok($"Пользователю {login} выданы права администратора");
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("/users/{login}")]
    public async Task<IActionResult> DeleteUser(string login, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        await mediator.Send(new DeleteUserCommand(user), cancellationToken);
        return Ok($"Пользователь {user.UserName} удален");
    }
    
    [HttpPost("/users/{login}/order")]
    public async Task<IActionResult> CreateUserOrder(string login, ShortOrderedGoodDto[] orderedGoods, CancellationToken cancellationToken)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        var orderId = await mediator.Send(new CreateOrderCommand(user.Id), cancellationToken);
        await mediator.Send(new CreateOrderedGoodsCommand(orderId, orderedGoods), cancellationToken);
        
        return Ok("Заказ создан");
    }
    
    [HttpGet("/users/{login}/orders")]
    public async Task<IActionResult> GetUserOrders(string login, CancellationToken cancellationToken)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var user = await userManager.FindByNameAsync(login);
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }
        
        var orders = await mediator.Send(new GetUserOrdersQuery(login), cancellationToken);
        return Ok(orders);
    }
    
    [HttpGet("/users/{login}")]
    public async Task<IActionResult> GetUser(string login, CancellationToken cancellationToken)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(User, login, "HaveAccess");
        if (!authorizationResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        
        var user = await mediator.Send(new GetUserQuery(login), cancellationToken);
        if(user == null)
        {
            return NotFound("Пользователь не найден");
        }
        return Ok(user);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(users);
    }
}