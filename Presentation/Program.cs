using Application.Extensions;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Extensions;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Presentation.Requirements.AccessRequirement;
using Presentation.Requirements.AccessRequirement.Handlers;
using Serilog;
using Serilog.Events;

namespace Presentation;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });
        
        builder.Services.AddSingleton<IAuthorizationHandler, RequestLoginIsUserLoginHandler>(); 
        builder.Services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("HaveAccess", policy =>
                policy.Requirements.Add(new AccessRequirement()));
        
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        
        builder.Services.AddSwaggerGen();
        
        var connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddHangfire(config => 
            config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connection)));
        builder.Services.AddHangfireServer();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        
        var app = builder.Build();
        

        app.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (httpContext, _, _) => 
                httpContext.Request.Path.StartsWithSegments("/hangfire/stats") ? 
                    LogEventLevel.Verbose : LogEventLevel.Information;
        });
        
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
        app.UseExceptionHandler();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();  
        app.UseAuthorization();
        
        app.MapControllers();
        app.MapHangfireDashboard();
        
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new [] {new HangfireAuthorizationFilter()}
        });
        
        app.Run();
    }
}