using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmBlitz.Cache
{
    public interface ICache
    {
        T Get<T>(string key);
        T Get<T>(string key, Func<T> func, int cacheTime = 30);
        bool Remove(string key);
        long Increment(string key, long value);
        long Decrement(string key, long value);
        bool Set<T>(string key, T data, int cacheTime = 30);
    }
}
