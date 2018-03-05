using AmBlitz.AutoMapping;
using AmBlitz.Dependency;
using AmBlitz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmBlitz.Module
{
    public class ModuleManager
    {
        private readonly List<AmBlitzModule> _amBlitzModules;

        private readonly Type _startAmBlitzModuleType;
        internal IocManager IocManager { get; }
        public ModuleManager(Type startAmBlitzModuleType, IocManager iocManager)
        {
            _startAmBlitzModuleType = startAmBlitzModuleType;
            _amBlitzModules = new List<AmBlitzModule>();
            IocManager = iocManager;
        }

        public void BulidAmBlitzModules()
        {
            //查找程序依赖的模块
            BuildAmBlitzModules(_startAmBlitzModuleType);
            var sortModules = _amBlitzModules.OrderByDescending(m => m.Sort).ToList();
            //实体对象信息管理
            EntityManager.Bulid(IocManager.ContainerBuilder, sortModules.Select(m=>m.AmBlitzModuleAssembly).ToArray());

            //各个模块初始化
            sortModules.ForEach(m=>m.PreInit());
            sortModules.ForEach(m => m.RegisterByConvention());
            sortModules.ForEach(m => m.Init());
            //对象转换
            MapperManager.BulidMapper(sortModules.Select(x => x.AmBlitzModuleAssembly).ToList());
        }

        private void BuildAmBlitzModules(Type amBlitzModuleType,int sort=0)
        {
           
            if (!AmBlitzModule.IsAmBlitzModule(amBlitzModuleType))
            {
                throw new ArgumentException("This type is not an AmBlitz module: " + amBlitzModuleType.AssemblyQualifiedName);
            }
            var amBlitzModule = _amBlitzModules.FirstOrDefault(m => m.AmBlitzModuleType == amBlitzModuleType);
            if (amBlitzModule == null)
            {
                var module = (AmBlitzModule) Activator.CreateInstance(amBlitzModuleType);
                module.AmBlitzModuleType = amBlitzModuleType;
                module.AmBlitzModuleAssembly = amBlitzModuleType.Assembly;
                module.Sort = sort;
                module.ContainerBuilder = IocManager.ContainerBuilder;
                _amBlitzModules.Add(module);
            }
            else
            {
                if (amBlitzModule.Sort<sort)
                {
                    amBlitzModule.Sort = sort;
                }
            }
            if (!amBlitzModuleType.IsDefined(typeof(DependAttribute), true))
            {
                return;
            }
            var dependsAttrs = amBlitzModuleType.GetCustomAttributes(typeof(DependAttribute), true)
                .Cast<DependAttribute>().ToList();

            foreach (var depend in dependsAttrs)
            {
                foreach (var dependtyModuleType in depend.DependedModuleTypes)
                {
                    BuildAmBlitzModules(dependtyModuleType, sort+1);
                }
            }
        }

    }
}
