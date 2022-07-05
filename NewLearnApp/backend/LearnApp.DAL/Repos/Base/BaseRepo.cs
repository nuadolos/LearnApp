using LearnApp.DAL.Context;
using LearnApp.DAL.Entities.Base;
using LearnApp.DAL.Entities.ErrorModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Repos.Base
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

        public async Task<int> AddAsync(T entity)
        {
            await _table.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<int> AddAsync(IList<T> entities)
        {
            await _table.AddRangeAsync(entities);
            return await SaveChangesAsync();
        }

        #endregion

        #region Обновление

        public async Task<int> UpdateAsync(T entity)
        {
            _table.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(IList<T> entities)
        {
            _table.UpdateRange(entities);
            return await SaveChangesAsync();
        }

        #endregion

        #region Удаление

        public async Task<int> DeleteAsync(int id, byte[] timestamp)
        {
            _db.Entry(new T() { Id = id, Timestamp = timestamp }).State = EntityState.Deleted;
            return await SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _table.Remove(entity);
            return await SaveChangesAsync();
        }

        #endregion

        #region Выборка

        public async Task<List<T>> GetAllAsync() => 
            await _table.ToListAsync();

        public async Task<T?> GetRecordAsync(int? id)
            => await _table.FindAsync(id);

        #endregion

        #region Сохранение изменений

        protected async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbMessageException(
                    "Запись, которую вы пытаетесь сохранить, была изменена другим пользователем", ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new DbMessageException(
                    "Привышен лимит попыток внесения данных в базу данных", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new DbMessageException(
                    "Возникла ошибка при сохранении в базе данных", ex);
            }
            catch (Exception ex)
            {
                throw new DbMessageException(
                    "Неизвестная ошибка. Мы пытаемся ее устранить", ex); 
            }
        }

        #endregion

        #region Освобождение ресурсов

        public void Dispose() => 
            _db.Dispose();

        #endregion
    }
}
