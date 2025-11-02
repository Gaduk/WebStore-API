using Application.Services;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Good.Commands.CreateGood;

public class CreateGoodCommandHandler(
    IGoodRepository      goodRepository,
    IFileService         fileService) : IRequestHandler<CreateGoodCommand, int>
{
    public async Task<int> Handle(CreateGoodCommand request, CancellationToken cancellationToken)
    {
        var good = new Domain.Entities.Good
        {
            Name  = request.Name,
            Price = request.Price
        };
        
        var goodId= await goodRepository.CreateGood(good, cancellationToken);
        var image= request.Image;

        if (image == null) return goodId;
        
        var url = await fileService.AddGoodImage(goodId, image, cancellationToken);
        
        if (string.IsNullOrEmpty(url)) return goodId;
        
        good.ImageUrls = good.ImageUrls.Concat(new[] {url}).ToArray();
        await goodRepository.UpdateGood(good, cancellationToken);

        return goodId;
    }
}