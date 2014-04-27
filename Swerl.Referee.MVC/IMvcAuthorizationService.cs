using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Core;

namespace Swerl.Referee.MVC
{
    public interface IMvcAuthorizationService : IAuthorizationService
    {
        bool Authorize(ActionExecutingContext context, IPrincipal user);
        bool Authorize(ActionExecutingContext context, IPrincipal user, bool handleFailure);
    }
}
