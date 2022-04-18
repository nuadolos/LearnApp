using LearnEF.Context;
using LearnEF.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Repos.Base
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        private readonly DbSet<T> _table;
        private readonly LearnContext _db;

        protected LearnContext Context => _db;

        public BaseRepo() : this(new LearnContext())
        { }

        public BaseRepo(LearnContext context)
        {
            _db = context;
            _table = context.Set<T>();
        }

        #region Добавление

        public int Add(T entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }

        public int Add(IList<T> entities)
        {
            _table.AddRange(entities);
            return SaveChanges();
        }

        #endregion

        #region Обновление

        public int Update(T entity)
        {
            _table.Update(entity);
            return SaveChanges();
        }

        public int Update(IList<T> entities)
        {
            _table.UpdateRange(entities);
            return SaveChanges();
        }

        #endregion

        #region Удаление

        public int Delete(int id, byte[] timestamp)
        {
            _db.Entry(new T() { Id = id, Timestamp = timestamp }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public int Delete(T entity)
        {
            _table.Remove(entity);
            return SaveChanges();
        }

        #endregion

        #region Выборка

        public List<T> GetAll() => 
            _table.ToList();

        public List<T> GetAll(Expression<Func<T, dynamic>> orderby, bool ascending)
            => ascending ? _table.OrderBy(orderby).ToList() : _table.OrderByDescending(orderby).ToList();

        public T GetRecord(int? id)
            => _table.Find(id);

        public List<T> GetWhere(Expression<Func<T, bool>> where)
            => _table.Where(where).ToList();

        public List<T> ExecuteQuery(string sql)
            => _table.FromSqlRaw(sql).ToList();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
            => _table.FromSqlRaw(sql, sqlParametersObjects).ToList();

        #endregion

        #region Сохранение изменений

        protected int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Освобождение ресурсов

        public void Dispose() => _db.Dispose();

        #endregion
    }
}
