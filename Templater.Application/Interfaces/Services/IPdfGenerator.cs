namespace Templater.Application.Interfaces.Services;

public interface IPdfGenerator
{
    byte[] GeneratePdfAsync(string html);
}