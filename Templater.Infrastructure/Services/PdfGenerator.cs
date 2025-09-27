using iText.Html2pdf;
using Templater.Application.Interfaces.Services;

namespace Templater.Infrastructure.Services;

public class PdfGenerator : IPdfGenerator
{
    public Task<byte[]> GeneratePdfAsync(string html)
    {
        using var ms = new MemoryStream();
        HtmlConverter.ConvertToPdf(html, ms);
        return Task.FromResult(ms.ToArray());
    }
}