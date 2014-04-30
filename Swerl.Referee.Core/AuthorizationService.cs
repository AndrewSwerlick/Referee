using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Resolvers;

namespace Swerl.Referee.Core
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizerResolver _authorizerResolver;

        public IActivityResolver ActivityResolver { get; private set; }

        public AuthorizationService(IAuthorizerResolver authorizerResolver, IActivityResolver activityResolver)
        {
            _authorizerResolver = authorizerResolver;
            ActivityResolver = activityResolver;
        }

        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return _authorizerResolver.GetAuthorizers(activity).All(a=> a.Authorize(activity, user));
        }

        public bool Authorize<T>(Expression<Action<T>> expression, IPrincipal user)
        {
            return Authorize((LambdaExpression) expression, user);
        }

        public bool Authorize(LambdaExpression expression, IPrincipal user)
        {
             var activity = ActivityResolver.GetActivity(expression);
            return _authorizerResolver.GetAuthorizers(expression).All(a => a.Authorize(activity, user));
        }
    }
}