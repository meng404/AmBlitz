using AmBlitz.Configuration;
using AmBlitz.Module;
using AmBlitz.RedisCache;

namespace AmBlitz.UnitTest
{
    [Depend(typeof(RedisCacheModule))]
    public class AmBlitzTestModule: AmBlitzModule
    {
        public override void PreInit()
        {
            //配置手脚架
            AmBlitzConfigurationBulider.Bulid(settings =>
            {
                settings
                .UseRedisDataBase("ebd.redis.31huiyi.com:6379");
            }, ContainerBuilder);
        }
    }
}
