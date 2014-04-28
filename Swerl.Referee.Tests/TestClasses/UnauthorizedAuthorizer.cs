using System.Security.Principal;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.Tests.TestClasses
{
    public class UnauthorizedAuthorizer : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return false;
        }
    }
}
