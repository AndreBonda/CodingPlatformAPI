using System.Linq;
using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly AppDbContext _dbCtx;
    
    protected BaseRepository(AppDbContext dbCtx)
    {
        _dbCtx = dbCtx;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(BaseFilters filters)
    {
        var results = _dbCtx.Set<TEntity>()
            .Take(filters.Take)
            .Skip(filters.Page);
        
        return await results.ToListAsync();
    }

    public async Task<TEntity> GetById(long id)
    {
        return await _dbCtx.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var inserted = await _dbCtx.Set<TEntity>().AddAsync(entity);
        await _dbCtx.SaveChangesAsync();
        return inserted.Entity;
    }

    public async Task<TEntity> DeleteAsync(long id)
    {
        var entity = await GetById(id);
        _dbCtx.Set<TEntity>().Remove(entity);
        await _dbCtx.SaveChangesAsync();
        return entity;
    }
}