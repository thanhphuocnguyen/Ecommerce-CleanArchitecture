using FluentValidation;

namespace Ecommerce.Application.Common.FileStorage;

public class FileUploadRequest
{
    public string FileName { get; set; } = default!;

    public string ContentType { get; set; } = default!;

    public byte[] Content { get; set; } = Array.Empty<byte>();
}

public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.ContentType).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
    }
}