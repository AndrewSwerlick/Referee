using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Swerl.Referee.Core;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Resolvers;

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

            MethodInfo info = null;
            if(descriptor is ReflectedActionDescriptor)
                info = ((ReflectedActionDescriptor)descriptor).MethodInfo;
            else if (descriptor is TaskAsyncActionDescriptor)
                info = ((TaskAsyncActionDescriptor) descriptor).TaskMethodInfo;
            else
                throw new InvalidOperationException("Action descriptor is not of an expected type. Unsure how to extract method information");

            var instance = Expression.Parameter(context.Controller.GetType(),"inst");
            var parameterExpressions = context.ActionParameters.Select(a =>
            {
                var parameterDescriptor = context.ActionDescriptor.GetParameters().Single(p => p.ParameterName == a.Key);
                var value = a.Value;
                var type = parameterDescriptor.ParameterType;
                //If the value is null, and the parameter type is a value type, Expression.Constant will throw an error, so we have to ensure value is set to a sane default. 
                //The MVC pipeline itself will throw an error later one anyways
                if (value == null)
                    value = type.IsValueType ? Activator.CreateInstance(type) : null;

                return Expression.Constant(value, parameterDescriptor.ParameterType);
            });

            var methodExpression = Expression.Call(instance, info, parameterExpressions);

            var expression = Expression.Lambda(methodExpression);

            var result = Authorize(expression, user);
            if(handleFailure && !result)
                _failureManager.HandleFailedAuthorization(expression,context);

            return result;
        }
    }
}
