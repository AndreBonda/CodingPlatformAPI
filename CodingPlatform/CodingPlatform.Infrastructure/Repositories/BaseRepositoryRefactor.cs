using System.Linq;
using CodingPlatform.AppCore.Filters;
using CodingPlatform.AppCore.Interfaces.Repositories;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Repositories;

public abstract class BaseRepositoryRefactor<TEntity> : IRepositoryRefactor<TEntity>
    where TEntity : BaseEntity
{
    protected readonly AppDbContext _dbCtx;

    protected BaseRepositoryRefactor(AppDbContext dbCtx)
    {
        _dbCtx = dbCtx;
    }

    public virtual async Task<TEntity> GetByIdAsync(long id) => await _dbCtx.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<TEntity> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return null;
        _dbCtx.Set<TEntity>().Remove(entity);
        await SaveAsync();
        return entity;
    }

    public async Task<bool> ExistAsync(long id) => await GetByIdAsync(id) != null;

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var inserted = await _dbCtx.Set<TEntity>().AddAsync(entity);
        await SaveAsync();
        return inserted.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity == null) return null;
        _dbCtx.Entry(dbEntity).CurrentValues.SetValues(entity);
        await SaveAsync();
        return dbEntity;
    }

    protected async Task SaveAsync() => await _dbCtx.SaveChangesAsync();
}