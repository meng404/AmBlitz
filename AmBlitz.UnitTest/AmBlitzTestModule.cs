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
    }
}
