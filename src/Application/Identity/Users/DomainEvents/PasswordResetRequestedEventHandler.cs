using Ecommerce.Domain.Common.Events;
using Ecommerce.Domain.Common.Mailing;

namespace Ecommerce.Domain.Identity.Users.DomainEvents;

public class PasswordResetRequestedEventHandler : EventNotificationHandler<PasswordResetRequestedEvent>
{
    private readonly IMailService _mailService;

    public PasswordResetRequestedEventHandler(IMailService mailService)
    {
        _mailService = mailService;
    }

    public override async Task Handle(PasswordResetRequestedEvent domainEvent, CancellationToken cancellationToken)
    {
        var mailRequest = new SendMailRequest(
            new List<string> { domainEvent.Email },
            "Reset Password",
            "Please reset your password by clicking <a href=\"" + domainEvent.ResetPasswordUrl + "\">here</a>.");

        await _mailService.SendEmailAsync(mailRequest, CancellationToken.None);
    }
}