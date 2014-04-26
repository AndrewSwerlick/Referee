using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public abstract class NamedQuery<T, TContext> where TContext : DbContext
    {
        public abstract T ExecuteQuery(TContext context);
    }
}
