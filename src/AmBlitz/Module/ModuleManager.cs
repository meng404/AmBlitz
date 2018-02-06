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
        private readonly List<AmBlitzModule> _AmBlitzModules;

        private readonly Type _startAmBlitzModuleType;
        internal IocManager _iocManager { get; private set; }
        public ModuleManager(Type startAmBlitzModuleType, IocManager iocManager)
        {
            _startAmBlitzModuleType = startAmBlitzModuleType;
            _AmBlitzModules = new List<AmBlitzModule>();
            _iocManager = iocManager;
        }

        public void BulidAmBlitzModules()
        {
            //查找程序依赖的模块
            BuildAmBlitzModules(_startAmBlitzModuleType);
            var sortModules = _AmBlitzModules.OrderByDescending(m => m.Sort).ToList();
            //实体对象信息管理
            EntityManager.Bulid(_iocManager.ContainerBuilder, sortModules.Select(m=>m.AmBlitzModuleAssembly).ToArray());

            //各个模块初始化
            sortModules.ForEach(m=>m.PreInit());
            sortModules.ForEach(m => m.RegisterByConvention());
            sortModules.ForEach(m => m.Init());
            //对象转换
            MapperManager.BulidMapper(sortModules.Select(x => x.AmBlitzModuleAssembly).ToList());
        }

        private void BuildAmBlitzModules(Type AmBlitzModuleType,int sort=0)
        {
           
            if (!AmBlitzModule.IsAmBlitzModule(AmBlitzModuleType))
            {
                throw new ArgumentException("This type is not an AmBlitz module: " + AmBlitzModuleType.AssemblyQualifiedName);
            }
            var amBlitzModule = _AmBlitzModules.FirstOrDefault(m => m.AmBlitzModuleType == AmBlitzModuleType);
            if (amBlitzModule == null)
            {
                var module = (AmBlitzModule) Activator.CreateInstance(AmBlitzModuleType);
                module.AmBlitzModuleType = AmBlitzModuleType;
                module.AmBlitzModuleAssembly = AmBlitzModuleType.Assembly;
                module.Sort = sort;
                module.ContainerBuilder = _iocManager.ContainerBuilder;
                _AmBlitzModules.Add(module);
            }
            else
            {
                if (amBlitzModule.Sort<sort)
                {
                    amBlitzModule.Sort = sort;
                }
            }
            if (!AmBlitzModuleType.IsDefined(typeof(DependAttribute), true))
            {
                return;
            }
            var dependsAttrs = AmBlitzModuleType.GetCustomAttributes(typeof(DependAttribute), true)
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
