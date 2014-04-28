using System;
using System.Linq.Expressions;
using System.Security.Principal;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core
{
    public interface IAuthorizationService
    {
        bool Authorize(IActivity activity, IPrincipal user);
        bool Authorize<T>(Expression<Action<T>> expression, IPrincipal user);
    }
}