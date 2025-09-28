namespace Templater.Application.Interfaces.Services;

public interface IAngleSharpHtmlSanitizer
{
    string Sanitize(string html);
    bool IsValidHtml(string html);
}