namespace Templater.Application.Interfaces.Services;

public interface IPdfGenerator
{
    Task<byte[]> GeneratePdfAsync(string html);
}