using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set(string key, object value);
        void Set(string key, object value, CachePolicy policy);
    }
}
