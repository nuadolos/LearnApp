using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos.Base
{
    public interface IRepo<T>
    {
        Task<int> AddAsync(T entity);
        Task<int> AddAsync(IList<T> entities);
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateAsync(IList<T> entities);
        Task<int> DeleteAsync(int id, byte[] timestamp);
        Task<int> DeleteAsync(T entity);
        Task<T?> GetRecordAsync(int? id);
        Task<List<T>> GetAllAsync();
    }
}
