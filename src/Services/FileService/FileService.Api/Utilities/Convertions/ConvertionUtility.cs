namespace FileService.Api.Utilities.Convertions;

public static class ConvertionUtility
{
    public static IFormFile ToFormFile(this byte[] data, string name)
    {
        MemoryStream stream = new MemoryStream(data);
        IFormFile file = new FormFile(stream, 0, data.Length, name, name);
        return file;
    }

    public static async Task<byte[]> GetBytes(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
