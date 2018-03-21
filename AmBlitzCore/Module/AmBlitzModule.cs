using AmBlitzCore.Application;
using AmBlitzCore.Dependency;
using AmBlitzCore.ToolKit.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AmBlitzCore.Module
{
    public abstract class AmBlitzModule
    {
        public IServiceCollection ServiceCollection { get; set; }
        public int Sort { get; set; }

        internal Type AmBlitzModuleType { get; set; }

        internal Assembly AmBlitzModuleAssembly { get; set; }

        public virtual void PreInit()
        {

        }

        public virtual void Init()
        {

        }

        public virtual void Shutdown()
        {

        }

        public virtual void ProcModules(List<AmBlitzModule> amBlitzModules)
        {

        }

        /// <summary>
        /// 判断是否是AmBlitzModule类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsAmBlitzModule(Type type)
        {
            return type.IsClass &&
                   !type.IsAbstract &&
                   !type.IsGenericType &&
                   typeof(AmBlitzModule).IsAssignableFrom(type);
        }

        internal void RegisterByConvention()
        {
            Register(ServiceLifetime.Transient);
            Register(ServiceLifetime.Scoped);
            Register(ServiceLifetime.Singleton);
        }

        private void Register(ServiceLifetime serviceLifetime)
        {
            var dependencies = GetDependencies(serviceLifetime);

            foreach (var type in dependencies)
            {
                var interfaceTypes = type.GetInterfaces().Where(m =>
                    m != typeof(ISingletonDependency) && m != typeof(ITransientDependency) &&
                    m != typeof(IScopedDependency) && m != typeof(IApplication));
                var enumerable = interfaceTypes as Type[] ?? interfaceTypes.ToArray();
                if (!enumerable.HasElement())
                {
                    ServiceCollection.TryAdd(new ServiceDescriptor(type, type, serviceLifetime));
                    continue;
                }
                foreach (var interfaceType in enumerable)
                {
                    ServiceCollection.TryAdd(new ServiceDescriptor(interfaceType, type, serviceLifetime));
                }
            }
        }

        private IEnumerable<Type> GetDependencies(ServiceLifetime serviceLifetime)
        {
            IEnumerable<Type> dependencies;
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    dependencies = AmBlitzModuleAssembly.GetExportedTypes()
                        .Where(m => typeof(ISingletonDependency).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract);
                    break;
                case ServiceLifetime.Scoped:
                    dependencies = AmBlitzModuleAssembly.GetExportedTypes()
                        .Where(m => typeof(IScopedDependency).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract);
                    break;
                case ServiceLifetime.Transient:
                    dependencies = AmBlitzModuleAssembly.GetExportedTypes()
                        .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }
            return dependencies;
        }
    }
}