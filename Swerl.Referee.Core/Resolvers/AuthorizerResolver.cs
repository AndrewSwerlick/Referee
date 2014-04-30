using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Extensions;
using Swerl.Referee.Core.Factories;

namespace Swerl.Referee.Core.Resolvers
{
    public class AuthorizerResolver : IAuthorizerResolver
    {
        private readonly IAuthorizerFactory _authorizerFactory;
        private readonly IEnumerable<IActivityRegistration> _activityRegistrations;

        public AuthorizerResolver(IAuthorizerFactory authorizerFactory,IEnumerable<IActivityRegistration> activityRegistrations)
        {
            var registrations = activityRegistrations as IActivityRegistration[] ?? activityRegistrations.ToArray();

            _authorizerFactory = authorizerFactory;
            _activityRegistrations = registrations;
        }

        public IList<IActivityAuthorizer> GetAuthorizers(IActivity activity)
        {
            IActivityRegistration registration = null;
            var registrations = _activityRegistrations.Where(a => a.ActivityType == activity.GetType()).ToList();
            IList<KeyValuePair<Type, Action<IActivityAuthorizer>>> types = null;

            if (registrations.Count() == 1)            
                registration = registrations.First();            

            if (registration == null && activity is MethodActivity)
                registration =
                    _activityRegistrations.SingleOrDefault(r => r.ActivityMethod == ((MethodActivity) activity).Info);

            if (registration == null)
                registration = _activityRegistrations.SingleOrDefault(r => r.ActivityName == activity.Name);

            if (registration != null)
                types = registration.AuthorizerTypes;

            if(types!= null && types.Any(a=> a.Key == typeof(AllowAnonymous)))
                types = new[]{new KeyValuePair<Type, Action<IActivityAuthorizer>>(typeof(AllowAnonymous),null)};

            if(types!= null)
                return BuildAuthorizers(types);

            return new[] {_authorizerFactory.BuildDefaultAuthorizer()};
        }

        public IList<IActivityAuthorizer> GetAuthorizers<T>(Expression<Action<T>> expression)
        {
            return GetAuthorizers((LambdaExpression)expression);
        }

        public IList<IActivityAuthorizer> GetAuthorizers(LambdaExpression expression)
        {
            IList<KeyValuePair<Type, Action<IActivityAuthorizer>>> types = null;

            var method = expression.GetMethodInfo();
            var registration =
                    _activityRegistrations.SingleOrDefault(r => r.ActivityMethod == method);

            if (registration != null)
                types = registration.AuthorizerTypes;

            if (types != null && types.Any(a => a.Key == typeof(AllowAnonymous)))
                types = new[] { new KeyValuePair<Type, Action<IActivityAuthorizer>>(typeof(AllowAnonymous), null) };

            if (types != null)
                return BuildAuthorizers(types);

            return new[] { _authorizerFactory.BuildDefaultAuthorizer() };
        }

        protected virtual IList<IActivityAuthorizer> BuildAuthorizers(IList<KeyValuePair<Type, Action<IActivityAuthorizer>>> types)
        {
            return types.Select(kv =>
            {
                var authorizer = _authorizerFactory.BuilAuthorizer(kv.Key);
                if(kv.Value != null)
                    kv.Value.Invoke(authorizer);

                return authorizer;
            }).ToList();
        }
    }
}
