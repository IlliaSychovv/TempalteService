using Microsoft.EntityFrameworkCore;
using Templater.Application.Interfaces;
using Templater.Application.Interfaces.Repositories;
using Templater.Domain.Entities;
using Templater.Infrastructure.Data;

namespace Templater.Infrastructure.Repositories;

public class TemplaterRepository : ITemplaterRepository
{
    private readonly AppDbContext _context;

    public TemplaterRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Template>> GetAllTemplatesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Templates.AsNoTracking().AsQueryable();

        return await query
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Template?> GetTemplateByIdAsync(Guid id)
    {
        return await _context.Templates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> CountTemplatesAsync()
    {
        return await _context.Templates.CountAsync();
    }

    public async Task AddTemplateAsync(Template template)
    {
        await _context.AddAsync(template);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTemplateAsync(Template template)
    {
        _context.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTemplateAsync(Guid id)
    {
        var template = await _context.Templates.FindAsync(id);
        
        _context.Templates.Remove(template);
        await _context.SaveChangesAsync();
    }
}