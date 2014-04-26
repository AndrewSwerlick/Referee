using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public class CachedQueryBuilder<T, TContext> : QueryBuilder<T, TContext> where TContext : DbContext
    {
        private readonly CacheableQuery<T, TContext> _query;
        private readonly ICacheProvider _cacheProvider;
        private bool _cacheAcrossContexts;

        public CachedQueryBuilder(TContext context, CacheableQuery<T, TContext> query, ICacheProvider cacheProvider)
            : base(context, query)
        {
            _query = query;
            _cacheProvider = cacheProvider;
        }

        public CachedQueryBuilder<T, TContext> CacheAcrossContexts()
        {
            _cacheAcrossContexts = true;
            return this;
        }

        public override T Execute()
        {
            var key = _query.GenerateCacheKey();
            
            var cachedResult = _cacheProvider.Get<T>(key);
            if (cachedResult != null)
                return cachedResult;

            var result = base.Execute();
            var policy = _query.GenerateCachePolicy();
            if(policy != null)
                _cacheProvider.Set(key, result, policy);
            else
                _cacheProvider.Set(key,result);

            return result;
        }
    }
}
