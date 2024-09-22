using Application.Services;

namespace Infrastructure.Services;

public class MailService(ILogger<MailService> logger) : IMailService
{
    public void SendMessage(string email)
    {
        logger.LogInformation("Mailing to {email}.", email);
    }
}
