namespace Ecommerce.Application.Common.Mailing;

public interface IMailService
{
    Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken);
}