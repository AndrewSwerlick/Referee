using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.NerdDinnerSample.Models;

namespace Swerl.Referee.NerdDinnerSample.Security.Authorizers
{
    public class RolesInDatabase : IActivityAuthorizer
    {
        private readonly ApplicationDbContext _context;

        public RolesInDatabase(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Authorize(IActivity activity, IPrincipal user)
        {
            var permissionEntry =  _context.ActivityPermissions.SingleOrDefault(a => a.Name == activity.Name);
            if (permissionEntry == null)
                return true;

            return permissionEntry.Roles.Any(r => user.IsInRole(r.Name));
        }
    }
}