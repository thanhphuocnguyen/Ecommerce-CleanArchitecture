using Ecommerce.Application.Common.FileStorage;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    public Result Remove(string path)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> UploadAsync<T>(FileUploadRequest request, FileType fileType)
    {
        throw new NotImplementedException();
    }
}
