using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public static class CacheProviderResolver
    {
        private static Func<ICacheProvider> _resolverFunc; 

        public static ICacheProvider GetCurrent()
        {
            return _resolverFunc.Invoke();
        }

        public static void SetResolver(Func<ICacheProvider> resolverFunc)
        {
            _resolverFunc = resolverFunc;
        }

        public static void SetResolver(ICacheProviderFactory factory)
        {
            _resolverFunc = factory.GetCacheProvider;
        }
    }
}
