using MediatR;

namespace Application.Interfaces;

public interface IMailService
{
    void SendMessage(string email);
}