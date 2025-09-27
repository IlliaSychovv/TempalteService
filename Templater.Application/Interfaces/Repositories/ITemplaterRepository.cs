using Templater.Domain.Entities;

namespace Templater.Application.Interfaces.Repositories;

public interface ITemplaterRepository
{
    Task<IReadOnlyList<Template>> GetAllTemplatesAsync(int pageNumber, int pageSize);
    Task<Template?> GetTemplateByIdAsync(Guid id);
    Task<int> CountTemplatesAsync();
    Task AddTemplateAsync(Template template);
    Task UpdateTemplateAsync(Template template);
    Task DeleteTemplateAsync(Guid id);
}