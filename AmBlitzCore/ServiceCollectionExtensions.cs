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
            foreach (var module in allDependModules)
            {
                module.RegisterByConvention(serviceCollection);
            }
            
        }
    }
}
