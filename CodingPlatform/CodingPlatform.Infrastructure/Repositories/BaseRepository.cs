using System.Linq;
using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly AppDbContext dbCtx;
    
    protected BaseRepository(AppDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(BaseFilters filters)
    {
        var results = dbCtx.Set<TEntity>()
            .Take(filters.Take)
            .Skip(filters.Page * filters.Take);
        
        return await results.ToListAsync();
    }

    public async Task<TEntity> GetById(long id)
    {
        return await dbCtx.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        entity.DateCreated = DateTime.UtcNow;
        var inserted = await dbCtx.Set<TEntity>().AddAsync(entity);
        await dbCtx.SaveChangesAsync();
        return inserted.Entity;
    }

    public async Task<TEntity> DeleteAsync(long id)
    {
        var entity = await GetById(id);
        dbCtx.Set<TEntity>().Remove(entity);
        await dbCtx.SaveChangesAsync();
        return entity;
    }

    protected IQueryable<TEntity> SetPagination(IQueryable<TEntity> query, BaseFilters filters)
    {
        query = query
            .Take(filters.Take)
            .Skip(filters.Page * filters.Take);
        return query;
    }
}