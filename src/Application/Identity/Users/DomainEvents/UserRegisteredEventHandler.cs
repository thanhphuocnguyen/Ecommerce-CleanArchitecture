using Ecommerce.Application.Common.Events;
using Ecommerce.Application.Common.Mailing;
using Ecommerce.Application.Identity.Users.Contracts;

namespace Ecommerce.Application.Identity.Users.DomainEvents;

public class UserRegisteredEventHandler : EventNotificationHandler<UserRegisteredEvent>
{
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IMailService _mailService;

    public UserRegisteredEventHandler(IEmailTemplateService emailTemplateService, IMailService mailService)
    {
        _emailTemplateService = emailTemplateService;
        _mailService = mailService;
    }

    public override async Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        var eMailModel = new RegisterUserEmailModel(domainEvent.Email, domainEvent.UserName, domainEvent.EmailVerificationUrl);

        var mailRequest = new SendMailRequest(
                [domainEvent.Email],
                "Confirm Registration",
                _emailTemplateService.GenerateEmailTemplate("email-confirmation", eMailModel));

        await _mailService.SendEmailAsync(mailRequest, CancellationToken.None);
    }
}