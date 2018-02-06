using Autofac;
using System;
namespace AmBlitz.Configuration
{
    /// <summary>
    ///   数据库配置  缓存配置等
    /// </summary>
    public static class AmBlitzConfigurationBulider
    {
        public static void Bulid(Action<AmBlitzConfigurationPart> settings, ContainerBuilder containerBuilder)
        {
            var pcp = new AmBlitzConfigurationPart();
            settings(pcp);
            //自我注册
            containerBuilder.RegisterInstance(pcp.Configuration).SingleInstance();
        }
    }


}
