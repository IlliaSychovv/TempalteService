using Mapster;
using SequentialGuid;
using Templater.Application.DTO;
using Templater.Application.Interfaces.Repositories;
using Templater.Domain.Entities;

namespace Templater.Application.Services;

public class TemplaterService : ITemplaterService
{
    private readonly ITemplaterRepository _templaterRepository;

    public TemplaterService(ITemplaterRepository repository)
    {
        _templaterRepository = repository;
    }

    public async Task<PagedResponse<TemplateDto>> GetTemplatesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var item = await _templaterRepository.GetAllTemplatesAsync(pageNumber, pageSize);
        var count = await _templaterRepository.CountTemplatesAsync();

        var itemsDto = item.Select(x => x.Adapt<TemplateDto>()).ToList();

        return new PagedResponse<TemplateDto>
        {
            Item = itemsDto,
            TotalCount = count,
            PageSize = pageSize
        };
    }

    public async Task<TemplateDto> GetTemplateById(Guid id)
    {
        var item = await _templaterRepository.GetTemplateByIdAsync(id);
        return item.Adapt<TemplateDto>();
    }
    
    public async Task<CreateTemplateDto> AddTemplateAsync(CreateTemplateDto dto)
    {
        var entity = dto.Adapt<Template>();
        entity.Id = SequentialGuidGenerator.Instance.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        
        await _templaterRepository.AddTemplateAsync(entity);
        return entity.Adapt<CreateTemplateDto>();
    }

    public async Task<bool> UpdateTemplateAsync(Guid id, UpdateTemplateDto dto)
    {
        var template = await _templaterRepository.GetTemplateByIdAsync(id);
        template.Name = dto.Name;
        template.HtmlContent = dto.HtmlContent; 
        template.ModifiedAt = DateTime.UtcNow;
        
        await _templaterRepository.UpdateTemplateAsync(template);
        return true;
    }

    public async Task<bool> DeleteTemplateAsync(Guid id)
    {
        await _templaterRepository.DeleteTemplateAsync(id);
        return true;
    }
}