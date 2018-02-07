using AmBlitz.Configuration;
using AmBlitz.Module;
using AmBlitz.Mongo;
using AmBlitz.RedisCache;
using System.Configuration;

namespace AmBlitz.UnitTest
{
    [Depend(typeof(RedisCacheModule), typeof(AmBlitzMongoModule))]
    public class AmBlitzTestModule: AmBlitzModule
    {
        public override void PreInit()
        {
            var masterDbcon = ConfigurationManager.AppSettings["MongodbHostPrimary"] ?? "";
            //配置手脚架
            AmBlitzConfigurationBulider.Bulid(settings =>
            {
                settings
                .MasterDataBases("EventBigData", masterDbcon)
                .UseRedisDataBase("ebd.redis.31huiyi.com:6379");
            }, ContainerBuilder);
        }
    }
}
