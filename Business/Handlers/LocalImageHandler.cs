using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Handlers;

public interface IImageHandler
{
    Task<string?> SaveProfileImageAsync(IFormFile file);
    Task<string?> SaveProjectImageAsync(IFormFile file);
}

public class LocalImageHandler(string imagePath) : IImageHandler
{
    private readonly string _imagePath = imagePath;

    public async Task<string?> SaveProjectImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null!;

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        if (!Directory.Exists(_imagePath))
            Directory.CreateDirectory(_imagePath);

        var filePath = Path.Combine(_imagePath, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }



    public async Task<string?> SaveProfileImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null!;

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        if (!Directory.Exists(_imagePath))
            Directory.CreateDirectory(_imagePath);

        var filePath = Path.Combine(_imagePath, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }
}
