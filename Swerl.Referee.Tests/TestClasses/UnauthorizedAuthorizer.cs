using System.Security.Principal;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;

namespace Swerl.Referee.UnitTests.TestClasses
{
    public class UnauthorizedAuthorizer : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return false;
        }
    }
}
