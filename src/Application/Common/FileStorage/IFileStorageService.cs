using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Application.Common.FileStorage;

public interface IFileStorageService
{
    Task<Result<string>> UploadAsync<T>(FileUploadRequest request, FileType fileType);

    Result Remove(string path);
}