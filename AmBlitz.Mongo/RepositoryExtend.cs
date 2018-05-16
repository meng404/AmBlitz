using AmBlitz.Domain;
using MongoDB.Driver;

namespace AmBlitz.Mongo
{
    public static  class RepositoryExtend
    {
        public static IMongoCollection<TEntity> MongoCollection<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository) where TEntity : class, IEntity<TPrimaryKey>
        {
            var rep = repository as RepositoryBase<TEntity, TPrimaryKey>;
            return rep?.MongoCollection;
        }
    }
}