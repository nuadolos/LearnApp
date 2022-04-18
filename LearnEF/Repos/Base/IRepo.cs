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
        int Add(T entity);
        int Add(IList<T> entities);
        int Update(T entity);
        int Update(IList<T> entities);
        int Delete(int id, byte[] timestamp);
        int Delete(T entity);
        T GetRecord(int? id);
        List<T> GetWhere(Expression<Func<T, bool>> where);
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, dynamic>> orderby, bool ascending);
        List<T> ExecuteQuery(string sql);
        List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);
    }
}
