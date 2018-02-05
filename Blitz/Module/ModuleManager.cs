using Blitz.AutoMapping;
using Blitz.Dependency;
using Blitz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blitz.Module
{
    public class ModuleManager
    {
        private readonly List<BlitzModule> _blitzModules;

        private readonly Type _startBlitzModuleType;
        internal IocManager _iocManager { get; private set; }
        public ModuleManager(Type startBlitzModuleType, IocManager iocManager)
        {
            _startBlitzModuleType = startBlitzModuleType;
            _blitzModules = new List<BlitzModule>();
            _iocManager = iocManager;
        }

        public void BulidBlitzModules()
        {
            //查找程序依赖的模块
            BuildBlitzModules(_startBlitzModuleType);
            var sortModules = _blitzModules.OrderByDescending(m => m.Sort).ToList();
            //实体对象信息管理
            EntityManager.Bulid(_iocManager.ContainerBuilder, sortModules.Select(m=>m.BlitzModuleAssembly).ToArray());

            //各个模块初始化
            sortModules.ForEach(m=>m.PreInit());
            sortModules.ForEach(m => m.RegisterByConvention());
            sortModules.ForEach(m => m.Init());

            //对象转换
            MapperManager.BulidMapper(sortModules.Select(x => x.BlitzModuleAssembly).ToList());
        }

        private void BuildBlitzModules(Type blitzModuleType,int sort=0)
        {
            if (!BlitzModule.IsBlitzModule(blitzModuleType))
            {
                throw new ArgumentException("This type is not an Blitz module: " + blitzModuleType.AssemblyQualifiedName);
            }
            var blitzModule = _blitzModules.FirstOrDefault(m => m.BlitzModuleType == blitzModuleType);
            if (blitzModule == null)
            {
                var module = (BlitzModule) Activator.CreateInstance(blitzModuleType);
                module.BlitzModuleType = blitzModuleType;
                module.BlitzModuleAssembly = blitzModuleType.Assembly;
                module.Sort = sort;
                module.ContainerBuilder = _iocManager.ContainerBuilder;
                _blitzModules.Add(module);
            }
            else
            {
                if (blitzModule.Sort<sort)
                {
                    blitzModule.Sort = sort;
                }
            }
            if (!blitzModuleType.IsDefined(typeof(DependAttribute), true))
            {
                return;
            }
            var dependsAttrs = blitzModuleType.GetCustomAttributes(typeof(DependAttribute), true)
                .Cast<DependAttribute>().ToList();

            foreach (var depend in dependsAttrs)
            {
                foreach (var dependtyModuleType in depend.DependedModuleTypes)
                {
                    BuildBlitzModules(dependtyModuleType, sort+1);
                }
            }
        }

    }
}
