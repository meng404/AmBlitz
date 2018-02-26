using AmBlitz.Cache;
using AmBlitz.Configuration;
using AmBlitz.Dependency;
using StackExchange.Redis;
using System;

namespace AmBlitz.RedisCache
{
    public class RedisClient:ICache, ISingletonDependency
    {
        private ConnectionMultiplexer _conn;
        private readonly string _connect;
        private const string Prefix = "amblitz";
        public RedisClient(AmBlitzConfiguration blitzConfiguration)
        {
            _connect = blitzConfiguration.RedisConnectionString();
        }

        private IDatabase GetDatabase()
        {
            if (_conn == null)
            {
                _conn = ConnectionMultiplexer.Connect(_connect);
            }
            return _conn.GetDatabase();
        }

        public long Decrement(string key, long value)
        {
            var db = GetDatabase();
            return db.StringDecrement(LocalKey(key), value);
        }

        public T Get<T>(string key)
        {
            var db = GetDatabase();
            var redisValue = db.StringGet(LocalKey(key));
            return redisValue.HasValue ? JsonSerializer.Deserialize<T>(redisValue) : default(T);
        }

        public T Get<T>(string key, Func<T> func, int cacheTime = 30)
        {
            var db = GetDatabase();
            var redisValue = db.StringGet(LocalKey(key));

            if (redisValue.HasValue)
            {
                return JsonSerializer.Deserialize<T>(redisValue);
            }
            var res = func();
            Set(key, res, cacheTime);
            return res;
        }

        public long Increment(string key, long value)
        {
            var db = GetDatabase();
            return db.StringIncrement(LocalKey(key), value);
        }

        public bool Remove(string key)
        {
            var db = GetDatabase();
            return db.KeyDelete(LocalKey(key));
        }

        public bool Set<T>(string key, T data, int cacheTime = 30)
        {
            var db = GetDatabase();
            return db.StringSet(LocalKey(key), JsonSerializer.Serialize(data), TimeSpan.FromMinutes(cacheTime));
        }

        private static string LocalKey(string key)
        {
            return Prefix + key;
        }
    }
}
