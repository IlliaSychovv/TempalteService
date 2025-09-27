namespace Templater.Application.DTO;

public record UpdateTemplateDto
{
    public string Name { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
}