using Application.Services;
using Hangfire;

namespace Infrastructure.Services;

public class MailService(ILogger<MailService> logger) : IMailService
{
    public void SubscribeToMailing(string email, string username)
    {
        RecurringJob.AddOrUpdate(
            $"SendEmailDailyTo_{username}", 
            () => SendMessage(email), 
            Cron.Daily);
    }

    public void SendMessage(string email)
    {
        logger.LogInformation("Mailing to {email}.", email);
    }
}
