namespace PTS.Persistence.Repositories;
public interface IRepository<TKey, TEntity> : IDisposable
    where TKey : struct
    where TEntity : class
{
    Task<TEntity> Create(TEntity entity);

    Task<IEnumerable<TEntity>> GetAll();

    Task<TEntity> Get(TKey id);

    Task<TEntity> Update(TKey id, TEntity entity);

    Task<bool> Delete(TKey id);

    Task Save();
}
