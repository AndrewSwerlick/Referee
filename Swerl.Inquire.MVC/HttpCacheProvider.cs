using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Swerl.Inquire.MVC
{
    public class HttpCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            return (T)HttpContext.Current.Cache.Get(key);
        }

        public void Set(string key, object value)
        {
            HttpContext.Current.Cache.Insert(key, value);
        }

        public void Set(string key, object value, CachePolicy policy)
        {
            if (!policy.AbsoluteExpiration.HasValue && !policy.SlidingExpiration.HasValue)
            {
                Set(key, value);
                return;
            }

            if(policy.AbsoluteExpiration.HasValue)
                HttpContext.Current.Cache.Insert(key,value,null, policy.AbsoluteExpiration.Value, Cache.NoSlidingExpiration);
            else
                HttpContext.Current.Cache.Insert(key, value, null,Cache.NoAbsoluteExpiration ,policy.SlidingExpiration.Value);
        }
    }
}
