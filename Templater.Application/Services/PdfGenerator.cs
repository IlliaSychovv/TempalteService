using iText.Html2pdf;
using Templater.Application.Interfaces.Services;

namespace Templater.Application.Services;

public class PdfGenerator : IPdfGenerator
{
    public byte[] GeneratePdfAsync(string html)
    {
        using var ms = new MemoryStream();
        HtmlConverter.ConvertToPdf(html, ms);
        return ms.ToArray();
    }
}