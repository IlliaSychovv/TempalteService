using PuppeteerSharp;
using PuppeteerSharp.Media;
using Templater.Application.Interfaces.Services;

namespace Templater.Infrastructure.Services;

public class PdfGenerator : IPdfGenerator
{
    public async Task<byte[]> GeneratePdfAsync(string html)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();  
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
        
        await using var page = await browser.NewPageAsync();
        
        await page.SetContentAsync(html);
        
        var pdfBytes = await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true
        });
        
        return pdfBytes;
    }
}