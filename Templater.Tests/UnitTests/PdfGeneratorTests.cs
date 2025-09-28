using Templater.Application.Services;

namespace TestProject1.UnitTests;

public class PdfGeneratorTests
{
    private readonly PdfGenerator _pdfGenerator;

    public PdfGeneratorTests()
    {
        _pdfGenerator = new PdfGenerator();
    }

    [Fact]
    public void GeneratePdfAsync_ShouldReturnNonEmptyByteArray_WhenHtmlIsValid()
    {
        string html = "<h1>Hello World</h1>";

        var result = _pdfGenerator.GeneratePdfAsync(html);

        Assert.NotNull(result);
        Assert.NotEmpty(result); 
    }
}