using AmBlitz.Domain;
using AmBlitz.Module;
using Autofac;

namespace AmBlitz.Mongo
{
    [Depend(typeof(KernelModule))]
    public class AmBlitzMongoModule: AmBlitzModule
    {
        public override void Init()
        {
            //泛型仓储注册
            ContainerBuilder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            ContainerBuilder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
