using System.Linq.Expressions;
using System.Web.Mvc;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.MVC
{
    public interface IAuthorizationFailureManager
    {
        void HandleFailedAuthorization(IActivity activity, ActionExecutingContext actionContext);
        void HandleFailedAuthorization(LambdaExpression expression, ActionExecutingContext actionContext);
    }
}