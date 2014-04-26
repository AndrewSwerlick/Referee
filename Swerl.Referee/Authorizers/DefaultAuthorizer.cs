using System.Security.Principal;
using Swerl.Referee.Activities;

namespace Swerl.Referee.Authorizers
{
    public class DefaultAuthorizer : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return true;
        }
    }
}
