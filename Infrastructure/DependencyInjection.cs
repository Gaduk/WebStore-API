using Application.Interfaces;
using Infrastructure.Services;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailService>();
    }
}