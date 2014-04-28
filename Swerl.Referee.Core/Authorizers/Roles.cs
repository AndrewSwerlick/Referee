using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Authorizers
{
    public class HasRoles  :IActivityAuthorizer
    {
        public HasRoles()
        {
            AuthorizedRoles = new string[]{};
        }

        public IList<string> AuthorizedRoles { get; set; } 
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return AuthorizedRoles.Any(user.IsInRole);
        }

        public void Roles(params string[] roles)
        {
            
        }
    }
}
