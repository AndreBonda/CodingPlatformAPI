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

    /*
     * Perchè questo? A differenza degli altri metodi base, il GetById non può essere implementato come default
     * poichè non sapre a priori gli Include() dell'EF da chiamare per mantenere tutte le invarianti, tuttavia mi piace
     * avere questo metodo nell'interfaccia IRepository.
     * Ho individuato due soluzioni:
     * a) Rendere il metodo astratto e implementato in ogni repository che lo eredità, tuttavia devo implementare il metodo
     *    anche nei repository che non lo usano.
     * b) Renderlo virtuale in modo tale che posso decidere di non overridarlo nei repository che lo ereditano, tuttavia se
     *    tale metodo viene invocato e non "overridato" si spacca.
     */
    public virtual Task<TEntity> GetByIdAsync(long id) => throw new NotSupportedException();

    public async Task DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        _dbCtx.Set<TEntity>().Remove(entity);
        await SaveAsync();
    }

    public async Task<bool> ExistAsync(long id) => await _dbCtx.Set<TEntity>().AnyAsync(e => e.Id == id);

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var inserted = await _dbCtx.Set<TEntity>().AddAsync(entity);
        await SaveAsync();
        return await GetByIdAsync(inserted.Entity.Id);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity == null) return null;
        _dbCtx.Entry(dbEntity).CurrentValues.SetValues(entity);
        await SaveAsync();
        return await GetByIdAsync(entity.Id);
    }

    protected async Task SaveAsync() => await _dbCtx.SaveChangesAsync();
}