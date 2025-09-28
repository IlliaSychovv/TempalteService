using Microsoft.AspNetCore.Mvc;
using Templater.Application.DTO;
using Templater.Application.Interfaces.Repositories;
using Templater.Application.Interfaces.Services;

namespace Templater.Controllers;

[ApiController]
[Route("api/v1/templater")]
public class TemplaterController : ControllerBase
{
    private readonly ITemplaterService _templaterService;
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IRazorTemplateRenderer _razorTemplateRenderer;

    public TemplaterController(ITemplaterService service, IPdfGenerator pdfGenerator,
        IRazorTemplateRenderer razorTemplateRenderer)
    {
        _templaterService = service;
        _pdfGenerator = pdfGenerator;
        _razorTemplateRenderer = razorTemplateRenderer;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<CreateTemplateDto>>> GetAllTemplates(int pageNumber = 1, int pageSize = 10)
    {
        var templates = await _templaterService.GetTemplatesAsync(pageNumber, pageSize);
        return Ok(templates);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(Guid id)
    {
        var template = await _templaterService.GetTemplateById(id);
        return Ok(template);
    } 
    
    [HttpPost("{id}/generate")]
    public async Task<IActionResult> GeneratePdf(Guid id, [FromBody] GeneratePdfDto dto)
    {
        var template = await _templaterService.GetTemplateById(id);
        if (template == null) 
            return NotFound();

        var html = await _razorTemplateRenderer.RenderAsync(template.HtmlContent, dto.Data);

        var pdfBytes = _pdfGenerator.GeneratePdfAsync(html);

        return File(pdfBytes, "application/pdf", $"{template.Name}.pdf");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateDto dto)
    { 
        var template = await _templaterService.AddTemplateAsync(dto);
        return Ok(template);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdateTemplateDto dto)
    {
        var template = await _templaterService.UpdateTemplateAsync(id, dto);
        if (!template)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(Guid id)
    { 
        var template = await _templaterService.DeleteTemplateAsync(id);
        if (!template)
            return NotFound();
        
        return NoContent();
    }
}