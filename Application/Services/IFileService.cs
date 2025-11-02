using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface IFileService
{
    Task<string> AddGoodImage(int goodId, IFormFile image, CancellationToken cancellationToken = default);
    Task<bool> DeleteGoodImages(int goodId, CancellationToken cancellationToken = default);
    Task<bool> DeleteGoodImage(string imagePath, CancellationToken cancellationToken = default);
}