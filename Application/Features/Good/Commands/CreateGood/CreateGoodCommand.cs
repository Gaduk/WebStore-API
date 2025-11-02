using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Good.Commands.CreateGood;

public record CreateGoodCommand(string Name, int Price, IFormFile? Image = null) : IRequest<int>;