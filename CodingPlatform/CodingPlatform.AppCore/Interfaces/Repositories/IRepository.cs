using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IRepository<TEntity>
where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(BaseSearch filters);
    Task<TEntity> GetByIdAsync(long id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> DeleteAsync(long id);
    Task<TEntity> UpdateAsync(TEntity entity);
}