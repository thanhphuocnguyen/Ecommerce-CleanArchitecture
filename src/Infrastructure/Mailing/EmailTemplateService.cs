using System.Text;
using Ecommerce.Domain.Common.Mailing;
using RazorEngineCore;

namespace Ecommerce.Infrastructure.Mailing;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailTemplate<T>(string templateName, T model)
    {
        string template = GetTemplate(templateName);

        IRazorEngine razorEngine = new RazorEngine();
        IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

        return modifiedTemplate.Run(model);
    }

    public static string GetTemplate(string templateName)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string tmpFolder = Path.Combine(baseDirectory, "EmailTemplates");
        string filePath = Path.Combine(tmpFolder, $"{templateName}.cshtml");

        using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = sr.ReadToEnd();
        sr.Close();

        return mailText;
    }
}