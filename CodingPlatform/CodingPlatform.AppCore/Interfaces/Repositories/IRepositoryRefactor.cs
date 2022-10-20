using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IRepositoryRefactor<TEntity>
    where TEntity : BaseEntity
{
    Task<bool> ExistAsync(long id);
    Task<TEntity> GetByIdAsync(long id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> DeleteAsync(long id);
    Task<TEntity> UpdateAsync(TEntity entity);
}