using Autofac;
using System;
namespace Blitz.Configuration
{
    /// <summary>
    ///   数据库配置  缓存配置等
    /// </summary>
    public static class BlitzConfigurationBulider
    {
        public static void Bulid(Action<BlitzConfigurationPart> settings, ContainerBuilder containerBuilder)
        {
            var pcp = new BlitzConfigurationPart();
            settings(pcp);
            //自我注册
            containerBuilder.RegisterInstance(pcp.Configuration).SingleInstance();
        }
    }


}
