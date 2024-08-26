using Application.Interfaces;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    public void SendMessage(string email)
    {
        Console.WriteLine($"Рассылка на почту {email}.");
    }
}