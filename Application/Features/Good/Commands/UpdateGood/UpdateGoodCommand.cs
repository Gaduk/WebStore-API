using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Good.Commands.UpdateGood;

public record UpdateGoodCommand(int Id, string Name, int Price, IFormFile? Image = null) : IRequest;