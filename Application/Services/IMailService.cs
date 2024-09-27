namespace Application.Services;

public interface IMailService
{
    void SubscribeToMailing(string email, string username);
}