using System.Security.Principal;
using Swerl.Referee.Activities;

namespace Swerl.Referee.Authorizers
{
    public interface IActivityAuthorizer
    {
        bool Authorize(IActivity activity, IPrincipal user);
    }
}
