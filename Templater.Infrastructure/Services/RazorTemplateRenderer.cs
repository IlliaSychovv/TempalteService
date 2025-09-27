using RazorLight;
using Templater.Application.Interfaces.Services;

namespace Templater.Infrastructure.Services;

public class RazorTemplateRenderer : IRazorTemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorTemplateRenderer()
    {
        _engine = new RazorLightEngineBuilder()
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderAsync(string templateContent, object model)
    {
        string templateKey = Guid.NewGuid().ToString();
        return await _engine.CompileRenderStringAsync(templateKey, templateContent, model);
    }
}