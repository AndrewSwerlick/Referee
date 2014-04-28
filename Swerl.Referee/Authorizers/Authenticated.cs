using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.NerdDinnerSample.Security.Authorizers
{
    public class Authenticated : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }
    }
}