using AmBlitz.Dependency;
using AmBlitz.Domain;
using MongoDB.Driver;

namespace AmBlitz.Mongo
{
    public interface IMongoDbProvider: ISingletonDependency
    {
        bool EnableSoftDelete<TEntity>();
        IMongoCollection<TEntity> MasterMongoCollection<TEntity>() where TEntity : class;
        IMongoCollection<TEntity> SlaveMongoCollection<TEntity>() where TEntity : class;
        EntityDescribe EntityDescribe<TEntity>();
    }
}
