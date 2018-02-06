using AmBlitz.Dependency;
using System;
using System.Runtime.Caching;
namespace AmBlitz.Cache
{
    public class AmBlitzMemoryCache: ICache, ISingletonDependency
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        public long Decrement(string key, uint value)
        {

            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            var x = Cache.Get(key);
            if (x == null)
            {
                return default(T);
            }
            return (T)x;
        }

        public T Get<T>(string key, Func<T> func, int cacheTime)
        {
            var x = Cache.Get(key);
            if (x == null)
            {
                var value = func();

                Set(key, value, cacheTime);

                return value;
            }
            return (T)x;
        }

        public long Increment(string key, uint value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            Cache.Remove(key);

            return true;
        }

        public bool Set<T>(string key, T data, int cacheTime)
        {
            return Cache.Add(key, data, DateTimeOffset.Now.AddMinutes(cacheTime));
        }
    }
}
