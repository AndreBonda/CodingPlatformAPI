using CodingPlatform.AppCore.Filters;
using CodingPlatform.Domain.Entities;

namespace CodingPlatform.AppCore.Interfaces.Repositories;

public interface IRepository<TEntity>
where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(BaseFilters filters);
    Task<TEntity> GetById(long id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> DeleteAsync(long id);
}