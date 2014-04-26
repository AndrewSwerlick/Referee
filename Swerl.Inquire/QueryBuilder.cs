using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Inquire
{
    public class QueryBuilder<T, TContext> where TContext : DbContext
    {
        protected readonly TContext Context;
        private readonly NamedQuery<T, TContext> _query;

        public QueryBuilder(TContext context, NamedQuery<T, TContext> query)
        {
            Context = context;
            _query = query;
        }

        public virtual T Execute()
        {
            return _query.ExecuteQuery(Context);
        }
    }
}
