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
        private readonly string connect;
        public RedisClient(AmBlitzConfiguration blitzConfiguration)
        {
            connect = blitzConfiguration.RedisConnectionString();
        }

        private IDatabase GetDatabase()
        {
            if (_conn == null)
            {
                _conn = ConnectionMultiplexer.Connect(connect);
            }
            return _conn.GetDatabase();
        }

        public long Decrement(string key, long value)
        {
            var db = GetDatabase();
            return db.StringDecrement(key, value);
        }

        public T Get<T>(string key)
        {
            var db = GetDatabase();
            var redisValue = db.StringGet(key);
            if (redisValue.HasValue)
            {
                return JsonSerializer.Deserialize<T>(redisValue);
            }
            return default(T);
        }

        public T Get<T>(string key, Func<T> func, int cacheTime = 30)
        {
            var db = GetDatabase();
            var redisValue = db.StringGet(key);

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
            return db.StringIncrement(key, value);
        }

        public bool Remove(string key)
        {
            var db = GetDatabase();
            return db.KeyDelete(key);
        }

        public bool Set<T>(string key, T data, int cacheTime = 30)
        {
            var db = GetDatabase();
            return db.StringSet(key, JsonSerializer.Serialize(data), TimeSpan.FromMinutes(cacheTime));
        }
    }
}
