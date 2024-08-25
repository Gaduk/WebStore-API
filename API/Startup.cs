using Application;
using Microsoft.AspNetCore.Authorization;
using Persistence;
using Web_API.Requirements.AccessRequirement;
using Web_API.Requirements.AccessRequirement.Handlers;

namespace Web_API;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication("Cookies").AddCookie(options => 
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
        services.AddApplication();
        services.AddPersistence(configuration);
        
        
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
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}