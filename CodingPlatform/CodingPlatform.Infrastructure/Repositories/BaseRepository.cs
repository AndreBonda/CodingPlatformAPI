using System.Linq;
using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly AppDbContext _dbCtx;

    protected BaseRepository(AppDbContext dbCtx)
    {
        _dbCtx = dbCtx;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(BaseSearch filters)
    {
        var results = _dbCtx.Set<TEntity>()
            .Take(filters.Take);

        return await results.ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(long id)
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
        var entity = await GetByIdAsync(id);
        _dbCtx.Set<TEntity>().Remove(entity);
        await _dbCtx.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity == null)
            return null;

        _dbCtx.Entry(dbEntity).CurrentValues.SetValues(entity);
        await _dbCtx.SaveChangesAsync();
        return dbEntity;
    }

    protected IQueryable<TEntity> SetPagination(IQueryable<TEntity> query, BaseSearch filters)
    {
        query = query
            .Take(filters.Take);
        return query;
    }
}