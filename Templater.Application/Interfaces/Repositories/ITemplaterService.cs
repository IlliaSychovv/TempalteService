using Templater.Application.DTO;

namespace Templater.Application.Interfaces.Repositories;

public interface ITemplaterService
{
    Task<PagedResponse<TemplateDto>> GetTemplatesAsync(int pageNumber = 1, int pageSize = 10);
    Task<TemplateDto> GetTemplateById(Guid id);
    Task<CreateTemplateDto> AddTemplateAsync(CreateTemplateDto dto);
    Task<bool> UpdateTemplateAsync(Guid id, UpdateTemplateDto dto);
    Task<bool> DeleteTemplateAsync(Guid id);
}