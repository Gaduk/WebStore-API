using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Web_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });
        
        //Entity Framework
        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

        //Аутентификация, авторизация
        builder.Services.AddAuthentication("Cookies").AddCookie(options => options.LoginPath = "/login");
        builder.Services.AddAuthorization();
        
        var app = builder.Build();

        //Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //Аутентификация, авторизация
        app.UseAuthentication();  
        app.UseAuthorization();


        app.MapGet("/login", async (HttpContext context) =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.SendFileAsync("Html/loginForm.html");
        });
        
        app.MapPost("/login", async (string? returnUrl, HttpContext context, ApplicationContext db) =>
        {
            // получаем из формы логин и пароль
            var form = context.Request.Form;
            // если логин и/или пароль не установлены, посылаем статусный код ошибки 400
            string? login = form["login"];
            string? password = form["password"];
            if (login == "" || password == "")
                return Results.BadRequest("Необходимо ввести логин и пароль в поля формы");
            
            // находим пользователя 
            User? user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            // если пользователь не найден, отправляем статусный код 401
            if (user is null) return Results.Unauthorized();
 
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            // установка аутентификационных куки
            await context.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
            return Results.Redirect(returnUrl??"/");
        });
 
        app.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/login");
        });
        
        app.MapGet("/order", async (HttpContext context) =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.SendFileAsync("Html/orderForm.html");
        });
        
        app.MapPost("/order", async (string? returnUrl, HttpContext context, ApplicationContext db) =>
        {
            // берем из кук данные пользователя
            //...
            
            var form = context.Request.Form;
            foreach (string goodName in form.Keys)
            {
                int goodAmount = int.Parse(form[goodName]);
                
                // добавляем заказ в таблицу
                //...
            }
            return Results.Redirect(returnUrl??"/");
        });
        
        app.MapGet("/users", [Authorize](ApplicationContext db) => db.Users.ToList())
            .WithMetadata(new SwaggerOperationAttribute(
                "Получить список пользователей", 
                "Возвращает список всех пользователей"));
        
        app.MapGet("/orders", (ApplicationContext db) => db.Orders.ToList())
            .WithMetadata(new SwaggerOperationAttribute(
                "Получить список заказов", 
                "Возвращает список всех заказов"));
        
        app.MapGet("/goods", (ApplicationContext db) => db.Goods.ToList())
            .WithMetadata(new SwaggerOperationAttribute(
                "Получить список товаров", 
                "Возвращает список всех товаров"));
        
        app.Run();
    }
}