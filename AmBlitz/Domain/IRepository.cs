using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmBlitz.Domain
{
    public interface IRepository<TEntity, in TPrimaryKey>  where TEntity : class, IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> Table { get; }
        bool Any(Expression<Func<TEntity, bool>> filter);
        bool IsUnique(TPrimaryKey id, string filedName, object value);
        long Count(Expression<Func<TEntity, bool>> filter);
        bool Remove(Expression<Func<TEntity, bool>> filter);
        bool Delete(Expression<Func<TEntity, bool>> filter);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> filter);
        void Insert(IEnumerable<TEntity> entities);
        void Insert(TEntity entity);
        bool Replace(TEntity entity);
        bool Update(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        bool UpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        TEntity FindOneAndUpdate(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        TEntity FindOneAndUpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);

        TEntity FindOneAndReplace(Expression<Func<TEntity, bool>> filter, TEntity entity);
        TEntity FindOneAndDelete(Expression<Func<TEntity, bool>> filter);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> IsUniqueAsync(TPrimaryKey id, string filedName, object value);
        Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter);
        Task InsertAsync(IEnumerable<TEntity> entities);
        Task InsertAsync(TEntity entity);
        Task<bool> ReplaceAsync(TEntity entity);
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        Task<bool> UpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        Task<TEntity> FindOneAndUpdateAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);
        Task<TEntity> FindOneAndUpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates);

        Task<TEntity> FindOneAndReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity entity);
        Task<TEntity> FindOneAndDeleteAsync(Expression<Func<TEntity, bool>> filter);

    }

    public interface IRepository<TEntity> : IRepository<TEntity, string> where TEntity : class, IEntity<string>
    {

    }
}
