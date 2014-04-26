using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public class MemoryCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            return (T) MemoryCache.Default.Get(key);
        }

        public void Set(string key, object value)
        {
            MemoryCache.Default.Set(key, value, new DateTimeOffset(DateTime.Now, new TimeSpan(1, 0, 0, 0)));
        }

        public void Set(string key, object value, CachePolicy policy)
        {
            if (!policy.AbsoluteExpiration.HasValue && !policy.SlidingExpiration.HasValue)
            {
                Set(key, value);
                return;
            }

            var policyItem = new CacheItemPolicy();

            if (policy.AbsoluteExpiration.HasValue)
                policyItem.AbsoluteExpiration = policy.AbsoluteExpiration.Value;

            if (policy.SlidingExpiration.HasValue)
                policyItem.SlidingExpiration = policy.SlidingExpiration.Value;

            MemoryCache.Default.Set(key, value, policyItem);
        }
    }    
}
