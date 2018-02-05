using Blitz.Dependency;
using MongoDB.Driver;

namespace Blitz.Domain
{
    public interface IMongoDbProvider: ISingletonDependency
    {
        bool EnableSoftDelete<TEntity>();
        IMongoCollection<TEntity> MasterMongoCollection<TEntity>() where TEntity : class;
        IMongoCollection<TEntity> SlaveMongoCollection<TEntity>() where TEntity : class;
        EntityDescribe EntityDescribe<TEntity>();
    }
}
