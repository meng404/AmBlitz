using System;
using AmBlitzCore.Module;
using Microsoft.Extensions.DependencyInjection;

namespace AmBlitzCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AutoBuild(this IServiceCollection serviceCollection,Type startType)
        {
            var allDependModules = ModuleManager.Instance.AmBlitzModules(startType);
            //模块注册
            foreach (var module in allDependModules)
            {
                module.ServiceCollection = serviceCollection;
                module.ProcModules(allDependModules);
                module.RegisterByConvention();
                module.PreInit();
                module.Init();
            }
            
        }
    }
}
