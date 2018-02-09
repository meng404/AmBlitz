using AmBlitz.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmBlitz.Application
{
    public interface ICurdApplication<TEntity>: IApplication where TEntity : class, IEntity<string>
    {
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void Insert(IEnumerable<TEntity> entitys);
        Task InsertAsync(IEnumerable<TEntity> entitys);
        bool Delete(Expression<Func<TEntity, bool>> filter);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter);
        bool Replace(TEntity entity);
        Task<bool> ReplaceAsync(TEntity entity);
        bool Update(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        bool UpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        Task<bool> UpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
    }
}