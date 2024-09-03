using Application.Interfaces;
using Hangfire;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    public void SendMessage(string login, string email)
    {
        RecurringJob.AddOrUpdate(
            $"SendEmailMinutelyTo_{login}", 
            () => Console.WriteLine($"Рассылка на почту {email}."), 
            Cron.Minutely);
    }
}