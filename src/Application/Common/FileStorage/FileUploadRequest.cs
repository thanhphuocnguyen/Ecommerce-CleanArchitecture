using FluentValidation;

namespace Ecommerce.Domain.Common.FileStorage;

public class FileUploadRequest
{
    public string Name { get; set; } = default!;

    public string Extension { get; set; } = default!;

    public string Data { get; set; } = default!;
}

public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Extension)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Data)
            .NotEmpty();
    }
}