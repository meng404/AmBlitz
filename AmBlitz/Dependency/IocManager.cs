using Autofac;
namespace AmBlitz.Dependency
{
    public class IocManager
    {
        public IContainer Container { get; private set; }
        public ContainerBuilder ContainerBuilder { get; }
        static IocManager()
        {
           
        }

        private IocManager()
        {
            ContainerBuilder = new ContainerBuilder();
        }
        public static IocManager Instance { get; } = new IocManager();

        public void InitContainer()
        {
            Container = ContainerBuilder.Build();
        }
    }
}
