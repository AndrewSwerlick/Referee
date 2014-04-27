using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Swerl.Referee.Activities;
using Swerl.Referee.Factories;
using Swerl.Referee.Resolvers;

namespace Swerl.Referee.Configuration
{
    public abstract class AbstractRefereeConfigurationBuilder<TRegistration> where TRegistration : ActivityRegistration<TRegistration>
    {
        private readonly IAuthorizerFactory _authorizerFactory;
        private readonly IActivityFactory _activityFactory;
        public IList<TRegistration> ActivityRegistrations { get; private set; }

        protected AbstractRefereeConfigurationBuilder(IAuthorizerFactory authorizerFactory, IActivityFactory activityFactory)
        {
            _authorizerFactory = authorizerFactory;
            _activityFactory = activityFactory;
            ActivityRegistrations = new List<TRegistration>();
        }

        public void Register(Func<TRegistration,TRegistration> acitivyRegistrationExpression)
        {
            var registration = acitivyRegistrationExpression.Invoke(BuildRegistration());
            if(registration.AuthorizerType == null)
                throw new InvalidRegistrationException();
            ActivityRegistrations.Add(registration);
        }       

        public RefereeConfiguration Build()
        {
            var authorizerResolver = new AuthorizerResolver(_authorizerFactory, ActivityRegistrations.Cast<ActivityRegistration>().ToList());
            var activityResolver = new ActivityResolver(_activityFactory, ActivityRegistrations.Cast<ActivityRegistration>());

            return new RefereeConfiguration(activityResolver, authorizerResolver);
        }

        public abstract TRegistration BuildRegistration();
    }
}
