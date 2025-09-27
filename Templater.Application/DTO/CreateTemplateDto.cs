namespace Templater.Application.DTO;

public record CreateTemplateDto
{
    public string Name { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
}