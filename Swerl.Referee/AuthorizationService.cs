using System;
using System.Linq.Expressions;
using System.Security.Principal;
using Swerl.Referee.Activities;
using Swerl.Referee.Resolvers;

namespace Swerl.Referee
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
            return _authorizerResolver.GetAuthorizer(activity).Authorize(activity, user);
        }

        public bool Authorize<T>(Expression<Action<T>> expression, IPrincipal user)
        {
            var activity = ActivityResolver.GetActivity(expression);
            return _authorizerResolver.GetAuthorizer(activity).Authorize(activity, user);
        }
    }
}