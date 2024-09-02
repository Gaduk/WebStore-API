using Application.Extensions;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Persistence.Extensions;
using Web_API.Requirements.AccessRequirement;
using Web_API.Requirements.AccessRequirement.Handlers;

namespace Web_API;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddPersistence(configuration);
        services.AddInfrastructure();
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
        {
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/accessDenied";
        });
        
        services.AddSingleton<IAuthorizationHandler, RequestLoginIsUserLoginHandler>(); 
        services.AddSingleton<IAuthorizationHandler, IsAdminHandler>(); 
        
        services.AddAuthorization(options => 
            {
                options.AddPolicy("HaveAccess", policy =>
                    policy.Requirements.Add(new AccessRequirement()));
            });
        
        services.AddControllers();
        
        services.AddSwaggerGen();
        
        var connection = configuration.GetConnectionString("DefaultConnection");
        
        services.AddHangfire(config => 
            config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connection)));
        services.AddHangfireServer();
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
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
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            }
        );
        app.UseHangfireDashboard();
    }
}