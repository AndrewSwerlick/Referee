using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Activities;
using Swerl.Referee.Resolvers;

namespace Swerl.Referee.MVC
{
    public class MvcAuthorizationService : AuthorizationService, IMvcAuthorizationService
    {
        private readonly IAuthorizationFailureManager _failureManager;

        public MvcAuthorizationService(IAuthorizerResolver authorizerResolver, IActivityResolver activityResolver, IAuthorizationFailureManager failureManager) : base(authorizerResolver, activityResolver)
        {
            _failureManager = failureManager;
        }

        public bool Authorize(ActionExecutingContext context, IPrincipal user)
        {
            return Authorize(context, user, false);
        }

        public bool Authorize(ActionExecutingContext context, IPrincipal user, bool handleFailure)
        {
            var resolver = ActivityResolver;
            IActivity activity = null;
            var descriptor = context.ActionDescriptor;

            var reflectedDescriptior = (ReflectedActionDescriptor)descriptor;
            var instance = Expression.Parameter(context.Controller.GetType(),"inst");
            var methodExpression = Expression.Call(instance, reflectedDescriptior.MethodInfo,
                context.ActionParameters.Select(a => Expression.Constant(a.Value)));

            var expression = Expression.Lambda(methodExpression);

            activity = resolver.GetActivity(expression);

            var result = Authorize(activity, user);
            if(handleFailure && !result)
                _failureManager.HandleFailedAuthorization(activity,context);

            return result;
        }
    }
}
