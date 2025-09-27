namespace Templater.Application.DTO;

public record GeneratePdfDto
{
    public Dictionary<string, object> Data { get; set; } = new();
}