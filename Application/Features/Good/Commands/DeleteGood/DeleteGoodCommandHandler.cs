using Application.Exceptions;
using Application.Services;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Good.Commands.DeleteGood;

public class DeleteGoodCommandHandler(IGoodRepository goodRepository, IFileService fileService)
    : IRequestHandler<DeleteGoodCommand>
{
    public async Task Handle(DeleteGoodCommand request, CancellationToken cancellationToken)
    {
        var good = await goodRepository.GetGood(request.GoodId, cancellationToken);
        if (good == null)
        {
            throw new NotFoundException($"Good ID {request.GoodId} is not found");
        }

        await goodRepository.DeleteGood(request.GoodId, cancellationToken);
        await fileService.DeleteGoodImages(request.GoodId, cancellationToken);
    }
}