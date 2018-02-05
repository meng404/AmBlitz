using AmBlitz.Dependency;
using AmBlitz.Module;
using System;
using System.Reflection;

namespace AmBlitz
{
    public class Bootstrapper : IDisposable
    {
        /// <summary>
        /// 入口模块
        /// </summary>
        internal Type StartupModule { get; }

        public IocManager IocManager { get; }

        private Bootstrapper(Type startupModule)
            : this(startupModule, IocManager.Instance)
        {

        }

        private Bootstrapper(Type startupModule, IocManager iocManager)
        {
            if (!typeof(AmBlitzModule).GetTypeInfo().IsAssignableFrom(startupModule))
            {
                throw new ArgumentException($"{nameof(startupModule)}应该继承于 {nameof(AmBlitzModule)}.");
            }

            StartupModule = startupModule;
            IocManager = iocManager;
        }
        public static Bootstrapper Create<TStartupModule>() where TStartupModule : AmBlitzModule
        {
            return new Bootstrapper(typeof(TStartupModule));
        }

        public void Initialize()
        {
            //处理项目依赖模块
            var moduleManager = new ModuleManager(StartupModule, IocManager);
            moduleManager.BulidAmBlitzModules();

            IocManager.InitContainer();
        }

        public void Dispose()
        {
            IocManager.Container.Dispose();
        }
    }
}
