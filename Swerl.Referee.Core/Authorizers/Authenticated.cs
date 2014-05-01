using System.Security.Principal;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Authorizers
{
    public class Authenticated : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }
    }
}