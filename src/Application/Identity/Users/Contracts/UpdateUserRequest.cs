using Ecommerce.Application.Common.FileStorage;

namespace Ecommerce.Application.Identity.Users.Contracts;

public class UpdateUserRequest
{
    public string Id { get; set; } = default!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public FileUploadRequest? Image { get; set; }

    public bool DeleteCurrentImage { get; set; } = false;
}