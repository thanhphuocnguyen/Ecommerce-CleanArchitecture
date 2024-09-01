using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Application.Common.FileStorage;

public interface IFileStorageService
{
    Task<Result<string>> UploadAsync<T>(FileUploadRequest request, FileType fileType, CancellationToken cancellationToken = default)
        where T : class;

    Result Remove(string path);
}