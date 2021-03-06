﻿using System.Security.Principal;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Authorizers
{
    public class AllowAnonymous : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return true;
        }
    }
}
