using Microsoft.AspNetCore.Http;

namespace OnlineShopAdmin.Common.Helpers;

public static class FileHelper
{
    private static void CreateDirectory(string folderPath)
    {
        if (folderPath == null) return;
        if (Directory.Exists(folderPath)) return;
        Directory.CreateDirectory(folderPath);
        var directory = new DirectoryInfo(folderPath);
        directory.Attributes = FileAttributes.Normal;
    }

    public static string SaveFile(IFormFile file, string folderPath, string fileName = null)
    {
        CreateDirectory(folderPath);

        fileName ??= file.FileName;

        // change filename if file exists
        var tmp = 0;
        var tmpPath = Path.Combine(folderPath, fileName);
        var cleanFileName = Path.GetFileNameWithoutExtension(fileName);
        var fileExtension = Path.GetExtension(file.FileName);
        var tmpFileName = fileName;

        while (File.Exists(tmpPath))
        {
            tmpFileName = $"{cleanFileName}({++tmp}){fileExtension}";
            tmpPath = Path.Combine(folderPath, tmpFileName);
        }

        fileName = tmpFileName;

        using var fileStream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
        file.CopyTo(fileStream);

        return fileName;
    }

    public static string DownloadFile(byte[] fileBytesArray)
    {
        if (fileBytesArray == null) return null;

        var file = Convert.ToBase64String(fileBytesArray);

        return file;
    }

    public static void DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
            return;

        if (!File.Exists(filePath))
            return;

        File.Delete(filePath);
    }

    public static async Task<byte[]> GetBytes(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}