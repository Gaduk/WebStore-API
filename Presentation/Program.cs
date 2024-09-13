using Application.Extensions;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Persistence.Context;
using Persistence.Extensions;
using Web_API.Requirements.AccessRequirement;
using Web_API.Requirements.AccessRequirement.Handlers;

namespace Web_API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddApplication();
        builder.Services.AddPersistence(builder.Configuration);
        
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
        {
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/accessDenied";
        });
        
        builder.Services.AddSingleton<IAuthorizationHandler, RequestLoginIsUserLoginHandler>(); 
        builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>(); 
        
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("HaveAccess", policy =>
                policy.Requirements.Add(new AccessRequirement()));
        
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        
        var connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddHangfire(config => 
            config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connection)));
        builder.Services.AddHangfireServer();
        
        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/swagger.json", "API V1");
                c.RoutePrefix = "swagger";
            });
        }
        app.UseRouting();
        app.UseAuthentication();  
        app.UseAuthorization();
        app.UseStaticFiles();
        
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new [] {new HangfireAuthorizationFilter()}
        });

        app.MapControllers();
        app.MapHangfireDashboard();

        app.Run();
    }
}