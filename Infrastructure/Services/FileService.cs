using Application.Services;

namespace Infrastructure.Services;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    /// <summary>
    /// Проверяет, является ли файл изображением по его расширению.
    /// Корректные расширения: .jpg, .jpeg, .png, .gif, .webp, .bmp, .svg.
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <returns>
    /// true - если расширение файла соответствует поддерживаемым форматам изображений;
    /// false - в противном случае
    /// </returns>
    /// <example>
    /// <code>
    /// bool isValid = IsImageFile("product.jpg"); // true
    /// bool isValid = IsImageFile("document.pdf"); // false
    /// </code>
    /// </example>
    private bool IsImageFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".bmp" or ".svg";
    }
    
    /// <summary>
    /// Добавляет изображение для указанного товара.
    /// </summary>
    /// <param name="goodId">Идентификатор товара</param>
    /// <param name="image">Файл изображения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>
    /// Относительный URL сохраненного изображения или пустую строку, 
    /// если файл не является изображением.
    /// </returns>
    public async Task<string> AddGoodImage(int goodId, IFormFile image, CancellationToken cancellationToken = default)
    {
        var goodDirectory = Path.Combine(environment.WebRootPath, "goods", $"{goodId}", "images");
        if (!Directory.Exists(goodDirectory))
        {
            Directory.CreateDirectory(goodDirectory);
        }

        var fileExtension = Path.GetExtension(image.FileName);
        var fileName           = $"good_{goodId}_{DateTime.Now:yyyy-MM-dd_HH.mm}_{Guid.NewGuid():N}{fileExtension}";
        var filePath           = Path.Combine(goodDirectory, fileName);

        if (!IsImageFile(filePath)) return "";
        
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(fileStream, cancellationToken);
            
        var url = Path.Combine("goods", $"{goodId}", "images", fileName);

        return url;
    }

    /// <summary>
    /// Удаляет ВСЕ изображения товара.
    /// </summary>
    /// <param name="goodId">Идентификатор товара</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>
    /// True, если удаление прошло успешно. Иначе - false.
    /// </returns>
    public Task<bool> DeleteGoodImages(int goodId, CancellationToken cancellationToken = default)
    {
        var imagesDirectory = Path.Combine(environment.WebRootPath, "goods", $"{goodId}", "images");

        if (!Directory.Exists(imagesDirectory)) return Task.FromResult(false);
        
        Directory.Delete(imagesDirectory, recursive: true);
        return Task.FromResult(true);
    }
    
    /// <summary>
    /// Удаляет изображение товара по указанному пути.
    /// </summary>
    /// <param name="imagePath">Путь к изображению от корня</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>
    /// True, если удаление прошло успешно. Иначе - false.
    /// </returns>
    public Task<bool> DeleteGoodImage(string imagePath, CancellationToken cancellationToken = default)
    {
        var absolutePath = Path.Combine(environment.WebRootPath, imagePath);

        if (!File.Exists(absolutePath)) return Task.FromResult(false);
        File.Delete(absolutePath);
        
        return Task.FromResult(true);
    }
}