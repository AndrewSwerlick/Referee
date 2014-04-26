using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire.MVC
{
    public class InqureConfig
    {
        public static void Configure()
        {
            CacheProviderResolver.SetResolver(()=> new HttpCacheProvider());
        }
    }
}
