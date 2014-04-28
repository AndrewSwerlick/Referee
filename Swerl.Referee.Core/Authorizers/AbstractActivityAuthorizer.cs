using System.Security.Principal;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Authorizers
{
    public abstract class AbstractActivityAuthorizer<T> : IActivityAuthorizer where T:IActivity
    {
        public abstract bool Authorize(T activity, IPrincipal user);

        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return Authorize((T)activity, user);
        }
    }
}
