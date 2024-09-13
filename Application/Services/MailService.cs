using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MailService(ILogger<MailService> logger) : IMailService
{
    public void SendMessage(string email)
    {
        logger.LogInformation("Рассылка на почту {email}.", email);
    }
}