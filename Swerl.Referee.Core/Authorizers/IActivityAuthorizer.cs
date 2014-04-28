using System.Security.Principal;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Authorizers
{
    public interface IActivityAuthorizer
    {
        bool Authorize(IActivity activity, IPrincipal user);
    }
}
