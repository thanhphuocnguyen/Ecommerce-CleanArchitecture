using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;
using Ecommerce.Infrastructure.Identity.Entities;
using Ecommerce.Infrastructure.Identity.Extensions;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a service for managing users.
/// </summary>
internal partial class UserService
{
    public async Task<Result<string>> CreateAsync(CreateUserRequest request, string origin)
    {
        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return ValidationResult<string>.WithErrors(result.GetErrors());
        }

        await _userManager.AddToRoleAsync(user, Roles.User);

        var messages = new List<string>
        {
            string.Format("User {0} Registered.", user.UserName)
        };

        // TODO:
        // if (!string.IsNullOrEmpty(user.Email))
        // {
        //     string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
        //     var eMailModel = new RegisterUserEmailModel(user.Email, user.UserName, emailVerificationUri);
        //     var mailRequest = new SendEmailRequest(new List<string> { user.Email },
        //         "Confirm Registration",
        //         _templateService.GenerateEmailTemplate("email-confirmation", eMailModel));
        //     _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
        //     messages.Add(_t[$"Please check {user.Email} to verify your account!"]);
        // }
        return Result<string>.Success(string.Join(Environment.NewLine, messages));
    }

    public async Task<Result> UpdateAsync(UpdateUserRequest request, UserId userId)
    {
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user == null)
        {
            return Result.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        string currentImage = user.ImageUrl ?? string.Empty;

        if (request.Image != null || request.DeleteCurrentImage)
        {
            var imageResult = await _fileStorageService.UploadAsync<AppUser>(request.Image!, FileType.Image);
            if (imageResult.IsFailure)
            {
                return Result.Failure(imageResult.Error);
            }

            user.ImageUrl = imageResult.Value;

            if (request.DeleteCurrentImage && !string.IsNullOrWhiteSpace(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                _fileStorageService.Remove(Path.Combine(root, currentImage));
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        var result = await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);

        // await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        if (!result.Succeeded)
        {
            return ValidationResult.WithErrors(result.GetErrors());
        }

        return Result.Success();
    }
}