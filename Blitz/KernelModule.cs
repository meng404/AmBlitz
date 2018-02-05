using Autofac;
using Blitz.Domain;
using Blitz.Logging;
using Blitz.Module;

namespace Blitz
{
    public class KernelModule: BlitzModule
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
