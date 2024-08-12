using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Web_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });
        
        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

        builder.Services.AddAuthentication("Cookies").AddCookie(options =>
        {
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/accessDenied";
        });
        builder.Services.AddAuthorization();
        
        
        var app = builder.Build();
        
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();  
        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapGet("/accessDenied", async (HttpContext context) =>
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Denied");
        });
        
        // Регистрация
        app.MapPost("/register", async (HttpContext context, ApplicationContext db, User user) =>
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return Results.Ok($"Пользователь {user.Login} успешно зарегистрирован");
        });
        
        // Вход в аккаунт
        app.MapPost("/login", async (HttpContext context, ApplicationContext db, string login, string password) =>
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return Results.BadRequest("Необходимо указать логин и пароль");
            
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            if (user is null) return Results.Unauthorized();

            string role = user.IsAdmin ? "admin" : "user";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, role)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await context.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
            return Results.Ok($"Произведен вход в аккаунт {login}");
        });
 
        // Выход из аккаунта
        app.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/login");
        });
        
        // Офромление заказа
        app.MapPost("{login}/order", [Authorize(Roles = "admin, user")]
            async (HttpContext context, ApplicationContext db, 
            string login, OrderedGood[] orderedGoods) =>
        {
            if (!RightsChecker.IsCookiesLogin(context, login) && !RightsChecker.IsAdmin(context))
                return Results.Forbid();
            
            if (!await RightsChecker.IsLoginAsync(db, login))
                return Results.NotFound("Пользователь не найден");
            
            var order = new Order
            {
                UserLogin = login,
                IsDone = false
            };
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
            int orderID = order.ID; 

            foreach (var orderedGood in orderedGoods)
            {
                if (orderedGood.Amount != 0)
                {
                    orderedGood.OrderID = orderID;
                    await db.OrderedGoods.AddAsync(orderedGood);
                }
            }
            await db.SaveChangesAsync();
            
            return Results.Ok("Заказ создан");
        });

        // Получение информации о пользователе
        app.MapGet("/{login}", [Authorize(Roles = "admin, user")]
            async (HttpContext context, ApplicationContext db, string login) =>
        {
            if (!RightsChecker.IsCookiesLogin(context, login) && !RightsChecker.IsAdmin(context))
                return Results.Forbid();
            
            if (!await RightsChecker.IsLoginAsync(db, login))
                return Results.NotFound("Пользователь не найден");

            User? user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
            return Results.Ok(user);
        });
        
        // Получение списка заказов пользователя
        app.MapGet("/{login}/orders", [Authorize(Roles = "admin, user")]
            async (HttpContext context, ApplicationContext db, string login) =>
        {
            if (!RightsChecker.IsCookiesLogin(context, login) && !RightsChecker.IsAdmin(context))
                return Results.Forbid();
            
            if (!await RightsChecker.IsLoginAsync(db, login))
                return Results.NotFound("Пользователь не найден");
            
            var orders = (from order in db.Orders.AsParallel().AsOrdered()
                where order.UserLogin == login
                select order).ToList();
            return Results.Ok(orders);
        });
        
        // Получение информации о конкретном заказе пользователя
        app.MapGet("/{login}/{orderID:int}", [Authorize(Roles = "admin, user")] 
            async (HttpContext context, ApplicationContext db, string login, int orderID) =>
        {
            if (!RightsChecker.IsCookiesLogin(context, login) && !RightsChecker.IsAdmin(context))
                return Results.Forbid();
            
            var order = await db.Orders.FirstOrDefaultAsync(o => o.UserLogin == login && o.ID == orderID);
            if (order == null)
                return Results.NotFound("У пользователя нет заказа с таким идентификатором, " +
                                        "либо пользователь с таким логином не существует.");
            /*
            var orderedGoods =
                db.OrderedGoods.FromSqlRaw(
                @"SELECT og.*, g.Name, g.Price FROM OrderedGoods og
                JOIN Goods g ON g.ID = og.GoodID
                WHERE og.orderID = {0}", orderID).ToList();

            var orderedGoods = (from og in db.OrderedGoods
                join g in db.Goods on og.GoodID equals g.ID
                where og.OrderID == orderID
                select og).ToList();
                */
            var orderedGoods = db.OrderedGoods
                .Join(db.Goods,
                    og => og.GoodID,
                    g => g.ID,
                    (og, g) => new
                    {
                        OrderID = og.OrderID, GoodID = og.GoodID, Amount = og.Amount,
                        Name = g.Name, Price = g.Price
                    }).ToList();
            
            orderedGoods = 
                (from og in orderedGoods
                where og.OrderID == orderID
                select og).ToList();
            
            return Results.Ok(orderedGoods);
            
        });
        
        // Изменение статуса заказа
        app.MapPut("{login}/{orderID:int}/status", [Authorize(Roles = "admin")] 
            async (HttpContext context, ApplicationContext db, string login, int orderID, bool isDone) => 
        {
            var order = await db.Orders.FirstOrDefaultAsync(o => o.UserLogin == login && o.ID == orderID);
            if (order == null)
                return Results.NotFound("У пользователя нет заказа с таким идентификатором, " +
                                        "либо пользователь с таким логином не существует.");
            order.IsDone = isDone;
            db.Orders.Update(order);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
        
        // Удаление пользователя вместе с данными о его заказах
        app.MapDelete("/user/{login}", [Authorize(Roles = "admin")] 
            async (HttpContext context, ApplicationContext db, string login) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null) return Results.NotFound("Пользователь не найден");
            
            var orders = (from order in db.Orders.AsParallel().AsOrdered()
                where order.UserLogin == login
                select order).ToList();
            
            var orderIds = orders.Select(o => o.ID).ToList(); 
            
            var orderedGoods = (from orderedGood in db.OrderedGoods.AsParallel().AsOrdered()
                where orderIds.Contains(orderedGood.OrderID)
                select orderedGood).ToList();
            
            db.OrderedGoods.RemoveRange(orderedGoods);
            db.Orders.RemoveRange(orders);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.Ok($"Пользователь {login} удален");
        });

        app.MapGet("/users", [Authorize(Roles = "admin")] async (HttpContext context, ApplicationContext db) =>
        {
            return db.Users.ToList();
        });

        app.MapGet("/orders", [Authorize(Roles = "admin")] async (HttpContext context, ApplicationContext db) =>
        {
            return db.Orders.ToList();
        });

        app.MapGet("/orderedGoods", [Authorize(Roles = "admin")] async (HttpContext context, ApplicationContext db) =>
        {
            return db.OrderedGoods.ToList();
        });

        app.MapGet("/goods", [Authorize(Roles = "admin")] async (HttpContext context, ApplicationContext db) =>
        {
            return db.Goods.ToList();
        });
        
        app.Run();
    }
}