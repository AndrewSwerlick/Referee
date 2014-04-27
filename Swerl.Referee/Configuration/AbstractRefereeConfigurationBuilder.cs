using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.Core.Resolvers;

namespace Swerl.Referee.Core.Configuration
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

            ValidateAndAddRegistration(registration);
        }

        public void RegisterClassMethods<TClass>(Func<TRegistration, TRegistration> acitivyRegistrationExpression)
        {
            var methods = typeof (TClass).GetMethods(BindingFlags.Instance|BindingFlags.DeclaredOnly|BindingFlags.Public).Where(m=> !m.IsSpecialName);
            foreach (var methodInfo in methods)
            {
                var registration = acitivyRegistrationExpression.Invoke(BuildRegistration());
                if(registration.ActivityMethod != null)
                    throw new InvalidRegistrationException("Cannot set the ActivityMethod when registering all methods in the class. Do not call the ActivityRegistration.Method method");
                registration.ActivityMethod = methodInfo;
                ValidateAndAddRegistration(registration);
            }
        }

        public RefereeConfiguration Build()
        {
            var authorizerResolver = new AuthorizerResolver(_authorizerFactory, ActivityRegistrations);
            var activityResolver = new ActivityResolver(_activityFactory, ActivityRegistrations);

            return new RefereeConfiguration(activityResolver, authorizerResolver);
        }

        public abstract TRegistration BuildRegistration();

        private void ModifyExistingRegistration(TRegistration existingRegistration, TRegistration newRegistration)
        {
            var typeChanged = existingRegistration.ActivityType != newRegistration.ActivityType &&
                              !(existingRegistration.ActivityType == null || newRegistration.ActivityType == null);

            if (typeChanged)
                throw new InvalidRegistrationException(
                    string.Format(
                        "Cannot register the same activity twice as two different activity types. Tried to register {0} as both {1} and {2}.",
                        newRegistration.ActivityMethod.Name, existingRegistration.ActivityType.FullName,
                        newRegistration.ActivityType.FullName));

            foreach (var authorizerType in newRegistration.AuthorizerTypes)
            {
                if(!existingRegistration.AuthorizerTypes.Contains(authorizerType))
                    existingRegistration.AuthorizerTypes.Add(authorizerType);
            }
        }

        private void ValidateAndAddRegistration(TRegistration registration)
        {
            if (!registration.AuthorizerTypes.Any())
                throw new InvalidRegistrationException("All registrations must have an AuthorizerType. Set this value by calling ActivityRegistration.AuthorizedBy");

            if (registration.ActivityMethod != null &&
                ActivityRegistrations.Any(a => a.ActivityMethod == registration.ActivityMethod))
            {
                ModifyExistingRegistration(
                    ActivityRegistrations.Single(a => a.ActivityMethod == registration.ActivityMethod), registration);
                return;
            }

            if (registration.ActivityMethod == null && registration.ActivityType != null &&
                ActivityRegistrations.Any(a => a.ActivityType == registration.ActivityType))
            {
                ModifyExistingRegistration(
                    ActivityRegistrations.Single(a => a.ActivityType == registration.ActivityType), registration);
                return;
            }

            if (registration.ActivityMethod == null && registration.ActivityName != null &&
                ActivityRegistrations.Any(a => a.ActivityName == registration.ActivityName))
            {
                ModifyExistingRegistration(
                    ActivityRegistrations.Single(a => a.ActivityName == registration.ActivityName), registration);
                return;
            }

            ActivityRegistrations.Add(registration);
        }
    }
}
