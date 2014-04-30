using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.NerdDinnerSample.Models;
using Swerl.Referee.NerdDinnerSample.Security.Activities;

namespace Swerl.Referee.NerdDinnerSample.Security.Authorizers
{
    public class EditDinnerAuthorizer : AbstractActivityAuthorizer<EditDinner>
    {
        private readonly ApplicationDbContext _context;

        public EditDinnerAuthorizer(ApplicationDbContext context)
        {
            _context = context;
        }

        public override bool Authorize(EditDinner activity, IPrincipal user)
        {
            var dinner = _context.Dinners.Find(activity.Id);
            return dinner.Host != null && dinner.Host.UserName == user.Identity.Name;
        }
    }
}