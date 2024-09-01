using Ecommerce.Application.Common.Mailing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Mailing;

internal static class DependencyInjection
{
    public static IServiceCollection AddMailingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        services.AddTransient<IEmailTemplateService, EmailTemplateService>();
        services.AddTransient<IMailService, SmtpMailService>();

        return services;
    }
}