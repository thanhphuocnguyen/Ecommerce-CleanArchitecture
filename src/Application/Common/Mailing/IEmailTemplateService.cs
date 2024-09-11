namespace Ecommerce.Domain.Common.Mailing;

public interface IEmailTemplateService
{
    string GenerateEmailTemplate<T>(string templateName, T model);
}