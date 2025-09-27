namespace Templater.Application.Interfaces.Services;

public interface IRazorTemplateRenderer
{
    Task<string> RenderAsync(string templateContent, object model);
}