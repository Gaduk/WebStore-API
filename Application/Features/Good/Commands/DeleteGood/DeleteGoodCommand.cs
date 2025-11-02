using MediatR;

namespace Application.Features.Good.Commands.DeleteGood;

public record DeleteGoodCommand(int GoodId) : IRequest;