namespace Templater.Application.DTO;

public record TemplateDto
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
    public DateTime CreatedAt{ get; set; }
    public DateTime? ModifiedAt{ get; set; }
}