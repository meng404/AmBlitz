using AmBlitz.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmBlitz.Mongo
{
    public class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IMongoDbProvider _dbProvider;

        //初始化
        public RepositoryBase(IMongoDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        //主库
        private IMongoCollection<TEntity> PrimaryMongoCollection => _dbProvider.MasterMongoCollection<TEntity>();

        //从库
        private IMongoCollection<TEntity> SlaveMongoCollection => _dbProvider.SlaveMongoCollection<TEntity>();

        //是否软删除
        private bool SoftDelete => _dbProvider.EnableSoftDelete<TEntity>();

        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return SoftDelete ? SlaveMongoCollection.AsQueryable().Where(i => ((ISoftDelete)i).IsDeleted == false) : SlaveMongoCollection.AsQueryable();
            }
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return SlaveMongoCollection.Find(SoftFilter(filter)).Any();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await SlaveMongoCollection.Find(SoftFilter(filter)).AnyAsync();
        }

        public virtual bool IsUnique(TPrimaryKey id, string filedName, object value)
        {

            FilterDefinition<TEntity> filter;
            if (SoftDelete)
            {
                filter = Builders<TEntity>.Filter.And(Builders<TEntity>.Filter.Eq(filedName, value),
                    Builders<TEntity>.Filter.Eq(i => ((ISoftDelete)i).IsDeleted, false));
            }
            else
            {
                filter = Builders<TEntity>.Filter.Eq(filedName, value);
            }
            if (id != null && !string.IsNullOrWhiteSpace(id.ToString()))
            {
                filter = Builders<TEntity>.Filter.And(filter, Builders<TEntity>.Filter.Ne(m => m.Id, id));
            }
            var find = SlaveMongoCollection.Find(filter);
            return find.FirstOrDefault() == null;
        }

        public virtual async Task<bool> IsUniqueAsync(TPrimaryKey id, string filedName, object value)
        {
            FilterDefinition<TEntity> filter;
            if (SoftDelete)
            {
                filter = Builders<TEntity>.Filter.And(Builders<TEntity>.Filter.Eq(filedName, value),
                    Builders<TEntity>.Filter.Eq(i => ((ISoftDelete)i).IsDeleted, false));
            }
            else
            {
                filter = Builders<TEntity>.Filter.Eq(filedName, value);
            }
            if (id != null && !string.IsNullOrWhiteSpace(id.ToString()))
            {
                filter = Builders<TEntity>.Filter.And(filter, Builders<TEntity>.Filter.Ne(m => m.Id, id));
            }
            var find = await SlaveMongoCollection.FindAsync(filter);
            return find.FirstOrDefault() == null;
        }

        public virtual long Count(Expression<Func<TEntity, bool>> filter)
        {
            return SlaveMongoCollection.Find(SoftFilter(filter)).Count();
        }
        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await SlaveMongoCollection.Find(SoftFilter(filter)).CountAsync();
        }
        public virtual bool Remove(Expression<Func<TEntity, bool>> filter)
        {
            return PrimaryMongoCollection.DeleteMany(filter).IsAcknowledged;
        }
        public virtual bool Delete(Expression<Func<TEntity, bool>> filter)
        {
            return SoftDelete
                ? PrimaryMongoCollection.UpdateMany(filter, Builders<TEntity>.Update.Set(i => ((ISoftDelete)i).IsDeleted, true)).IsAcknowledged
                : PrimaryMongoCollection.DeleteMany(filter).IsAcknowledged;
        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (SoftDelete)
            {
                var x = await PrimaryMongoCollection.UpdateManyAsync(filter,
                    Builders<TEntity>.Update.Set(i => ((ISoftDelete)i).IsDeleted, true));
                return x.IsAcknowledged;
            }
            else
            {
                var x = await PrimaryMongoCollection.DeleteManyAsync(filter);
                return x.IsAcknowledged;
            }
        }

        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> filter)
        {
            return SlaveMongoCollection.Find(SoftFilter(filter)).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            var x = await SlaveMongoCollection.FindAsync(SoftFilter(filter));
            return x.ToList();
        }

        public virtual TEntity FindOneAndUpdate(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Set(upitem.Key, upitem.Value)).ToList();

            return PrimaryMongoCollection.FindOneAndUpdate(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups));
        }

        public virtual async Task<TEntity> FindOneAndUpdateAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Set(upitem.Key, upitem.Value)).ToList();

            return await PrimaryMongoCollection.FindOneAndUpdateAsync(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups));
        }

        public virtual TEntity FindOneAndReplace(Expression<Func<TEntity, bool>> filter, TEntity entity)
        {
            return PrimaryMongoCollection.FindOneAndReplace(SoftFilter(filter), entity);
        }

        public virtual async Task<TEntity> FindOneAndReplaceAsync(Expression<Func<TEntity, bool>> filter, TEntity entity)
        {
            return await PrimaryMongoCollection.FindOneAndReplaceAsync(SoftFilter(filter), entity);
        }

        public virtual TEntity FindOneAndDelete(Expression<Func<TEntity, bool>> filter)
        {
            return PrimaryMongoCollection.FindOneAndDelete(SoftFilter(filter));
        }

        public virtual async Task<TEntity> FindOneAndDeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await PrimaryMongoCollection.FindOneAndDeleteAsync(SoftFilter(filter));
        }
        public virtual TEntity FindOneAndUpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Inc(upitem.Key, upitem.Value)).ToList();
            var options = new FindOneAndUpdateOptions<TEntity, TEntity>
            {
                ReturnDocument = ReturnDocument.After
            };
            return PrimaryMongoCollection.FindOneAndUpdate(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups), options: options);
        }

        public virtual async Task<TEntity> FindOneAndUpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Inc(upitem.Key, upitem.Value)).ToList();
            var options = new FindOneAndUpdateOptions<TEntity, TEntity>
            {
                ReturnDocument = ReturnDocument.After
            };
            var x = await PrimaryMongoCollection.FindOneAndUpdateAsync(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups), options: options);
            return x;
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return SlaveMongoCollection.Find(SoftFilter(filter)).FirstOrDefault();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            var x = await SlaveMongoCollection.FindAsync(SoftFilter(filter));
            return x.FirstOrDefault();
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            PrimaryMongoCollection.InsertMany(entities);
        }

        public virtual void Insert(TEntity entity)
        {
            PrimaryMongoCollection.InsertOne(entity);
        }

        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            return PrimaryMongoCollection.InsertManyAsync(entities);
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            return PrimaryMongoCollection.InsertOneAsync(entity);
        }

        public virtual bool Replace(TEntity entity)
        {
            return PrimaryMongoCollection.ReplaceOne(CreateEqualityExpressionForId(entity.Id), entity).IsAcknowledged;
        }

        public virtual async Task<bool> ReplaceAsync(TEntity entity)
        {
            var x = await PrimaryMongoCollection.ReplaceOneAsync(CreateEqualityExpressionForId(entity.Id), entity);
            return x.IsAcknowledged;
        }

        public virtual bool Update(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Set(upitem.Key, upitem.Value)).ToList();
            return PrimaryMongoCollection.UpdateMany(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups)).IsAcknowledged;
        }

        public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Set(upitem.Key, upitem.Value)).ToList();
            var x = await PrimaryMongoCollection.UpdateManyAsync(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups));
            return x.IsAcknowledged;
        }

        public bool UpdateInc(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Inc(upitem.Key, upitem.Value)).ToList();
            return PrimaryMongoCollection.UpdateMany(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups)).IsAcknowledged;
        }

        public async Task<bool> UpdateIncAsync(Expression<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var ups = updates.Select(upitem => Builders<TEntity>.Update.Inc(upitem.Key, upitem.Value)).ToList();
            var x = await PrimaryMongoCollection.UpdateManyAsync(SoftFilter(filter), Builders<TEntity>.Update.Combine(ups));
            return x.IsAcknowledged;
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        private FilterDefinition<TEntity> SoftFilter(Expression<Func<TEntity, bool>> filter)
        {
            if (!SoftDelete) return Builders<TEntity>.Filter.Where(filter);

            var where = Builders<TEntity>.Filter.Where(filter);
            var soft = Builders<TEntity>.Filter.Where(i => ((ISoftDelete)i).IsDeleted == false);
            return Builders<TEntity>.Filter.And(@where, soft);
        }
    }
    /// <summary>
    ///  默认MongoDB 主键
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity> : RepositoryBase<TEntity, string>, IRepository<TEntity> where TEntity : class, IEntity<string>
    {
        private readonly IMongoDbProvider _mongoDbProvider;
        private readonly IBusinessPrimaryKeyGen _businessPrimaryKeyGen;
        public RepositoryBase(IMongoDbProvider dbProvider, IBusinessPrimaryKeyGen businessPrimaryKeyGen) : base(dbProvider)
        {
            _mongoDbProvider = dbProvider;
            _businessPrimaryKeyGen = businessPrimaryKeyGen;
        }
        private EntityDescribe _entityDescribe => _mongoDbProvider.EntityDescribe<TEntity>();
        public override void Insert(IEnumerable<TEntity> entities)
        {
            if (_entityDescribe.BusinessPrimaryKeyAttribute != null)
            {
                foreach (var entity in entities)
                {
                    var value = _businessPrimaryKeyGen.Gen(_entityDescribe.BusinessPrimaryKeyAttribute.BusinessPrimaryKeyType);
                    _entityDescribe.BusinessPrimaryKeyAttribute.KeyDescriptor.SetValue(entity, value);
                }
            }
            base.Insert(entities);
        }
        public override void Insert(TEntity entity)
        {
            if (_entityDescribe.BusinessPrimaryKeyAttribute != null)
            {
                var value = _businessPrimaryKeyGen.Gen(_entityDescribe.BusinessPrimaryKeyAttribute.BusinessPrimaryKeyType);
                _entityDescribe.BusinessPrimaryKeyAttribute.KeyDescriptor.SetValue(entity, value);
            }
            base.Insert(entity);
        }
        public override Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (_entityDescribe.BusinessPrimaryKeyAttribute != null)
            {
                foreach (var entity in entities)
                {
                    var value = _businessPrimaryKeyGen.Gen(_entityDescribe.BusinessPrimaryKeyAttribute.BusinessPrimaryKeyType);
                    _entityDescribe.BusinessPrimaryKeyAttribute.KeyDescriptor.SetValue(entity, value);
                }
            }
            return base.InsertAsync(entities);
        }
        public override Task InsertAsync(TEntity entity)
        {
            if (_entityDescribe.BusinessPrimaryKeyAttribute != null)
            {
                var value = _businessPrimaryKeyGen.Gen(_entityDescribe.BusinessPrimaryKeyAttribute.BusinessPrimaryKeyType);
                _entityDescribe.BusinessPrimaryKeyAttribute.KeyDescriptor.SetValue(entity, value);
            };
            return base.InsertAsync(entity);
        }
    }
}
