using AmBlitz.Module;

namespace AmBlitz.RedisCache
{
    [Depend(typeof(KernelModule))]
    public class RedisCacheModule: AmBlitzModule
    {
    }
}
