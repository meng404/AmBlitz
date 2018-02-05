using Autofac;
using AmBlitz.Domain;
using AmBlitz.Logging;
using AmBlitz.Module;

namespace AmBlitz
{
    public class KernelModule: AmBlitzModule
    {
        public override void PreInit()
        {
            ContainerBuilder.RegisterModule(new Log4NetModule());
        }
        public override void Init()
        {
            //泛型仓储注册
            ContainerBuilder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            ContainerBuilder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
