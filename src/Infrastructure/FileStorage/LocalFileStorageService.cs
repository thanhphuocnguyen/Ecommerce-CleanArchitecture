using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ecommerce.Application.Common.FileStorage;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;
using Ecommerce.Infrastructure.Data.Extensions;

namespace Ecommerce.Infrastructure.FileStorage;

public class LocalFileStorageService() : IFileStorageService
{
    public Result Remove(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Result.Success();
    }

    public async Task<Result<string>> UploadAsync<T>(FileUploadRequest request, FileType fileType, CancellationToken cancellationToken)
        where T : class
    {
        if (request is null || request.Data is null)
        {
            return Result<string>.Failure(DomainErrors.FileStorage.FileUploadRequestIsInvalid);
        }

        if (request.Extension is null || !fileType.GetDescriptionList().Exists(x => x == request.Extension.ToLower()))
        {
            return Result<string>.Failure(DomainErrors.FileStorage.FileExtensionIsInvalid);
        }

        if (string.IsNullOrEmpty(request.Name))
        {
            return Result<string>.Failure(DomainErrors.FileStorage.FileNameIsInvalid);
        }

        string base64Data = Regex.Match(request.Data, "data:image;/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
        if (streamData.Length > 0)
        {
            string folder = typeof(T).Name;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                folder = folder.Replace(@"\", "/");
            }

            string folderName = fileType switch
            {
                FileType.Image => Path.Combine("Files", "Images", folder),
                _ => Path.Combine("Files", "Others", folder)
            };
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Directory.CreateDirectory(pathToSave);

            string fileName = request.Name.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhiteSpace("-");
            fileName += request.Extension.Trim();
            string fullPath = Path.Combine(pathToSave, fileName);
            string dbPath = Path.Combine(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            using var stream = new FileStream(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            return dbPath.Replace("\\", "/");
        }
        else
        {
            return string.Empty;
        }
    }

    public static string RemoveSpecialCharacters(string text)
    {
        return Regex.Replace(text, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    private const string NumberPattern = "-{0}";

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        if (Path.HasExtension(path))
        {
            return GetNextFileName(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
        }

        return GetNextFileName(path + NumberPattern);
    }

    private static string GetNextFileName(string v)
    {
        string tmp = string.Format(v, 1);

        if (!File.Exists(tmp))
        {
            return tmp;
        }

        int min = 1, max = 2;
        while (File.Exists(string.Format(v, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(v, pivot)))
            {
                min = pivot;
            }
            else
            {
                max = pivot;
            }
        }

        return string.Format(v, max);
    }
}