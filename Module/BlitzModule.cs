using Autofac;
using Blitz.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blitz.Module
{
    public abstract class BlitzModule
    {
        public ContainerBuilder ContainerBuilder { get; internal set; }

        public int Sort { get; set; }

        internal Type BlitzModuleType { get; set; }

        internal Assembly BlitzModuleAssembly { get; set; }

        public virtual void PreInit()
        {

        }

        public virtual void Init()
        {

        }

        /// <summary>
        /// 判断是否是BlitzModule类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsBlitzModule(Type type)
        {
            return type.IsClass &&
                   !type.IsAbstract &&
                   !type.IsGenericType &&
                   typeof(BlitzModule).IsAssignableFrom(type);
        }
        internal void RegisterByConvention()
        {
            Register(DependencyLifeStyle.Transient);
            Register(DependencyLifeStyle.Scoped);
            Register(DependencyLifeStyle.Singleton);
        }

        private void Register(DependencyLifeStyle serviceLifetime)
        {
            IEnumerable<Type> dependencies;
            switch (serviceLifetime)
            {
                case DependencyLifeStyle.Singleton:
                    dependencies = BlitzModuleAssembly.GetExportedTypes()
                        .Where(m => typeof(ISingletonDependency).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract);
                    foreach (var type in dependencies)
                    {
                        ContainerBuilder.RegisterType(type).AsImplementedInterfaces().SingleInstance();
                    }
                    break;
                case DependencyLifeStyle.Scoped:
                    dependencies = BlitzModuleAssembly.GetExportedTypes()
                        .Where(m => typeof(IScopedDependency).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract);
                    foreach (var type in dependencies)
                    {
                        ContainerBuilder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
                    }
                    break;
                case DependencyLifeStyle.Transient:
                    dependencies = BlitzModuleAssembly.GetExportedTypes()
                        .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract);
                    foreach (var type in dependencies)
                    {
                        ContainerBuilder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerDependency();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }
        }
    }
}
