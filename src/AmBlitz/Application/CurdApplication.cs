using AmBlitz.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmBlitz.Application
{
    public abstract class CurdApplication<TEntity> : ICurdApplication<TEntity> where TEntity:class, IEntity<string>
    {
        private readonly IRepository<TEntity> _repository;

        protected CurdApplication(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }
        public virtual Task InsertAsync(TEntity entity)
        {
            return _repository.InsertAsync(entity);
        }

        public virtual void Insert(IEnumerable<TEntity> entitys)
        {
            _repository.Insert(entitys);
        }
        public virtual Task InsertAsync(IEnumerable<TEntity> entitys)
        {
           return _repository.InsertAsync(entitys);
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.Delete(filter);
        }
        public virtual Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.DeleteAsync(filter);
        }

        public virtual bool Replace(TEntity entity)
        {
           return _repository.Replace(entity);
        }
        public virtual Task<bool> ReplaceAsync(TEntity entity)
        {
            return _repository.ReplaceAsync(entity);
        }

        public virtual bool Update(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
           return  _repository.Update(filter, updates);
        }

        public virtual Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            return _repository.UpdateAsync(filter, updates);
        }
        public virtual bool UpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            return _repository.UpdateInc(filter, updates);
        }
        public virtual Task<bool> UpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            return _repository.UpdateIncAsync(filter, updates);
        }
        public TEntity FindOneAndUpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            return _repository.FindOneAndUpdateInc(filter, updates);
        }
        public async Task<TEntity> FindOneAndUpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            return await _repository.FindOneAndUpdateIncAsync(filter, updates);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.FirstOrDefault(filter);
        }
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.FirstOrDefaultAsync(filter);
        }
        /// <inheritdoc />
        /// <summary>
        /// 获取所有符合条件的 (小心使用）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>IEnumerable<TEntity /></returns>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.FindAll(filter);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _repository.FindAllAsync(filter);
        }

    }
}
