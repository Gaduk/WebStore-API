using System.Reflection;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Services;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddSingleton<IMailService, MailService>();
    }
}