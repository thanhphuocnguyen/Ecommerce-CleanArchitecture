using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.Domain.Common.FileStorage;

public interface IFileStorageService
{
    Task<Result<string>> UploadAsync<T>(FileUploadRequest request, FileType fileType, CancellationToken cancellationToken = default)
        where T : class;

    Result Remove(string path);
}