﻿using System;
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
                throw new InvalidRegistrationException("All registrations must have an AuthorizerType. Set this value by calling ActivityRegistration.AuthorizedBy");

            if(registration.ActivityMethod != null && ActivityRegistrations.Any(a=> a.ActivityMethod == registration.ActivityMethod))
                throw new InvalidRegistrationException("Cannot register the same method twice");

            if(registration.ActivityMethod == null && registration.ActivityType != null && ActivityRegistrations.Any(a=> a.ActivityType == registration.ActivityType))
                throw new InvalidRegistrationException("Cannot register the same type twice unless you specify a different method. Either remove one registration or specify unique methods with the ActivityRegistration.Method method");

            if (registration.ActivityMethod == null && registration.ActivityName != null && ActivityRegistrations.Any(a => a.ActivityName == registration.ActivityName))
                throw new InvalidRegistrationException("Cannot register the same type name unless you specify a different method. Either remove one registration or specify unique methods with the ActivityRegistration.Method method");

            ActivityRegistrations.Add(registration);
        }       

        public RefereeConfiguration Build()
        {
            var authorizerResolver = new AuthorizerResolver(_authorizerFactory, ActivityRegistrations);
            var activityResolver = new ActivityResolver(_activityFactory, ActivityRegistrations);

            return new RefereeConfiguration(activityResolver, authorizerResolver);
        }

        public abstract TRegistration BuildRegistration();
    }
}
