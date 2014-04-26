using System.Data.Entity;

namespace Swerl.Inquire
{
    public abstract class CacheableQuery<T, TContext> : NamedQuery<T, TContext> where TContext : DbContext
    {
        public abstract string GenerateCacheKey();
        public abstract CachePolicy GenerateCachePolicy();
    }
}