namespace Ecommerce.Domain.Common.Mailing;

public interface IMailService
{
    Task SendEmailAsync(SendMailRequest request, CancellationToken cancellationToken);
}