using Ecommerce.Application.Common.Mailing;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ecommerce.Infrastructure.Mailing;

public class SmtpMailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<SmtpMailService> _logger;

    public SmtpMailService(IOptions<MailSettings> mailSettings, ILogger<SmtpMailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(SendMailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, request.From ?? _mailSettings.From));

            foreach (var address in request.To)
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            if (!string.IsNullOrEmpty(request.ReplyTo))
            {
                email.ReplyTo.Add(new MailboxAddress(request.ReplyToName, request.ReplyTo));
            }

            if (request.Bcc != null)
            {
                foreach (var address in request.Bcc.Where(bccVal => !string.IsNullOrWhiteSpace(bccVal)))
                {
                    email.Bcc.Add(MailboxAddress.Parse(address.Trim()));
                }
            }

            if (request.Cc != null)
            {
                foreach (var address in request.Cc.Where(ccVal => !string.IsNullOrWhiteSpace(ccVal)))
                {
                    email.Cc.Add(MailboxAddress.Parse(address.Trim()));
                }
            }

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    email.Headers.Add(header.Key, header.Value);
                }
            }

            var builder = new BodyBuilder();
            email.Sender = new MailboxAddress(request.DisplayName ?? _mailSettings.DisplayName, request.From ?? _mailSettings.From);
            email.Subject = request.Subject;
            builder.HtmlBody = request.Body;

            if (request.AttachmentData != null)
            {
                foreach (var attachment in request.AttachmentData)
                {
                    builder.Attachments.Add(attachment.Key, attachment.Value);
                }
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}