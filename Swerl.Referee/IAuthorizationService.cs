using System;
using System.Linq.Expressions;
using System.Security.Principal;
using Swerl.Referee.Activities;

namespace Swerl.Referee
{
    public interface IAuthorizationService
    {
        bool Authorize(IActivity activity, IPrincipal user);
        bool Authorize<T>(Expression<Action<T>> expression, IPrincipal user);
    }
}