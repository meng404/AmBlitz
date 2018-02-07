using AmBlitz.Cache;
using AmBlitz.Configuration;
using AmBlitz.Dependency;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmBlitz.RedisCache
{
    public class RedisClient:ICache, ISingletonDependency
    {
        private ConnectionMultiplexer _conn;
        private int DbNum { get; }

        public RedisClient(AmBlitzConfiguration blitzConfiguration)
        {
            var connstr = "";
            foreach (var conn in blitzConfiguration.GetRedisConfigurations())
            {
                connstr = connstr + conn + ",";
            }
            connstr = connstr.TrimEnd(',');
            _conn = ConnectionMultiplexer.Connect(connstr);
        }

        private IDatabase GetDatabase()
        {
            if (_conn == null)
            {
                var connstr = "";
                _conn = ConnectionMultiplexer.Connect(connstr);
            }
            return _conn.GetDatabase();
        }

        private  string ConvertJson<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }


        public long Decrement(string key, long value)
        {
            var db = GetDatabase();
            return db.StringDecrement(key, value);
        }

        public T Get<T>(string key)
        {
            var db = GetDatabase();
            var value = db.StringGet(key);
            if (value == RedisValue.Null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public T Get<T>(string key, Func<T> func, int cacheTime = 30)
        {
            var db = GetDatabase();
            var value = db.StringGet(key);
            if (value == RedisValue.Null)
            {
                var objValue = func();
                Set(key, objValue, cacheTime);
                return objValue;
            }
            return JsonConvert.DeserializeObject<T>(value);
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
            var json = ConvertJson(data);
            return db.StringSet(key, json, TimeSpan.FromMinutes(cacheTime));
        }
    }
}
