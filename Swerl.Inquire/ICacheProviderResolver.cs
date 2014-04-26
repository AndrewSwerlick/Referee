using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public interface ICacheProviderFactory
    {
        ICacheProvider GetCacheProvider();
    }
}
