namespace Ecommerce.Application.Common.Mailing;

public interface IMailService
{
    Task SendEmailAsync(SendMailRequest request, CancellationToken cancellationToken);
}