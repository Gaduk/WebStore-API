using Application.Exceptions;
using Application.Services;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Good.Commands.UpdateGood;

public class UpdateGoodCommandHandler(IGoodRepository goodRepository, IFileService fileService)
    : IRequestHandler<UpdateGoodCommand>
{
    public async Task Handle(UpdateGoodCommand request, CancellationToken cancellationToken)
    {
        var good = await goodRepository.GetGood(request.Id, cancellationToken);
        if (good == null)
        {
            throw new NotFoundException($"Good ID {request.Id} is not found");
        }

        await fileService.DeleteGoodImages(request.Id, cancellationToken);

        if (request.Image != null)
        {
            var url = await fileService.AddGoodImage(request.Id, request.Image, cancellationToken);
            good.ImageUrls = [url];
        }

        good.Name  = request.Name;
        good.Price = request.Price;
        
        await goodRepository.UpdateGood(good, cancellationToken);
    }
}