using AmBlitz.Logging;
using AmBlitz.Module;
using Autofac;

namespace AmBlitz
{
    public class KernelModule: AmBlitzModule
    {
        public override void PreInit()
        {
            ContainerBuilder.RegisterModule(new Log4NetModule());
        }
    }
}
