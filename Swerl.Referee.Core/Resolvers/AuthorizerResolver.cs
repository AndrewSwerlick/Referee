using System;
using System.Collections.Generic;
using System.Linq;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
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

            if (registrations.Count() == 1)            
                registration = registrations.First();            

            if (registration == null && activity is MethodActivity)
                registration =
                    _activityRegistrations.SingleOrDefault(r => r.ActivityMethod == ((MethodActivity) activity).Info);

            if (registration == null)
                registration = _activityRegistrations.SingleOrDefault(r => r.ActivityName == activity.Name);

            if (registration != null)
                return BuildAuthorizers(registration.AuthorizerTypes);

            return new[] {_authorizerFactory.BuildDefaultAuthorizer()};
        }

        protected virtual IList<IActivityAuthorizer> BuildAuthorizers(IDictionary<Type, Action<IActivityAuthorizer>> types)
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
