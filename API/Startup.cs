using Application;
using Persistence;

namespace Web_API;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddAuthentication("Cookies").AddCookie(options => 
        {
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/accessDenied";
        });
        services.AddAuthorization(options => 
            {
                options.AddPolicy(
                    "HaveUserRights",
                    policyBuilder => policyBuilder
                        .RequireClaim("Role", "User"));
                options.AddPolicy(
                    "HaveAdminRights",
                    policyBuilder => policyBuilder
                        .RequireClaim("Role", "Admin"));
            });
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
            });
        }
        app.UseRouting();
        app.UseAuthentication();  
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}