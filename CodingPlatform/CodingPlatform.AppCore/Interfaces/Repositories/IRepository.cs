using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<bool> ExistAsync(long id);
    Task<TEntity> GetByIdAsync(long id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task DeleteAsync(long id);
    Task<TEntity> UpdateAsync(TEntity entity);
}