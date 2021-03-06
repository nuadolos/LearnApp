using LearnApp.DAL.Entities.Base;

namespace LearnApp.DAL.Repos.Base
{
    public interface IRepo<T> where T : EntityBase
    {
        Task<int> AddAsync(T entity);
        Task<int> AddAsync(IList<T> entities);
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateAsync(IList<T> entities);
        Task<int> DeleteAsync(Guid id, byte[] timestamp);
        Task<int> DeleteAsync(T entity);
        Task<T?> GetRecordAsync(Guid id);
        Task<List<T>> GetAllAsync();
    }
}
