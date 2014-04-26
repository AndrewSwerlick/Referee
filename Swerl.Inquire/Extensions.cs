using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public static class Extensions
    {
        public static QueryBuilder<T, TContext> Query<TContext, T>(this TContext context, NamedQuery<T, TContext> query)
            where TContext : DbContext
        {
            return new QueryBuilder<T, TContext>(context, query);
        }

        public static CachedQueryBuilder<T, TContext> QueryCache<TContext, T>(this TContext context, CacheableQuery<T, TContext> query)
            where TContext : DbContext
        {
            return new CachedQueryBuilder<T, TContext>(context, query, CacheProviderResolver.GetCurrent());
        }
    }
}
