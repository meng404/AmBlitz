using System;
using System.Collections.Generic;
using System.Linq;
using AmBlitzCore.ToolKit.Extensions;

namespace AmBlitzCore.Module
{
    internal sealed class ModuleManager
    {
        private List<AmBlitzModule> _amBlitzModules;
        static ModuleManager()
        {

        }
        private ModuleManager()
        {
        }
        public static ModuleManager Instance { get; } = new ModuleManager();

        public List<AmBlitzModule> AmBlitzModules(Type startBlitzModuleType)
        {
            if (_amBlitzModules.HasElement())
            {
                return _amBlitzModules;
            }
            FindAmBlitzModules(startBlitzModuleType);
            _amBlitzModules = _amBlitzModules.OrderByDescending(m => m.Sort).ToList();
            return _amBlitzModules;
        }

        private void FindAmBlitzModules(Type amBlitzModuleType, int sort = 0)
        {

            if (!AmBlitzModule.IsAmBlitzModule(amBlitzModuleType))
            {
                throw new ArgumentException("This type is not an AmBlitz module: " + amBlitzModuleType.AssemblyQualifiedName);
            }
            var amBlitzModule = _amBlitzModules.FirstOrDefault(m => m.AmBlitzModuleType == amBlitzModuleType);
            if (amBlitzModule == null)
            {
                var module = (AmBlitzModule)Activator.CreateInstance(amBlitzModuleType);
                module.AmBlitzModuleType = amBlitzModuleType;
                module.AmBlitzModuleAssembly = amBlitzModuleType.Assembly;
                module.Sort = sort;
                _amBlitzModules.Add(module);
            }
            else
            {
                if (amBlitzModule.Sort < sort)
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
                    FindAmBlitzModules(dependtyModuleType, sort + 1);
                }
            }
        }
    }
}
