using AmBlitz.Dependency;
using Autofac;
using System;
namespace AmBlitz.Configuration
{
    /// <summary>
    ///   数据库配置  缓存配置等
    /// </summary>
    public static class AmBlitzGlobalConfiguration
    {
        public static void Configuration(Action<AmBlitzConfigurationPart> settings)
        {
            var pcp = new AmBlitzConfigurationPart();
            settings(pcp);
            //自我注册
            IocManager.Instance.ContainerBuilder.RegisterInstance(pcp.Configuration).SingleInstance();
        }
    }


}
