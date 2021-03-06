﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public MultiRegistration RegisterEach<T>(params Expression<Action<T>>[] methods)
        {
            var registrations = methods.Select(m => BuildRegistration().Method(m)).ToList();
            return new MultiRegistration(registrations, this);
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

            if (existingRegistration.ActivityType == null && newRegistration.ActivityType != null)
                existingRegistration.ActivityType = newRegistration.ActivityType;

            foreach (var authorizerType in newRegistration.AuthorizerTypes)
            {
                if(existingRegistration.AuthorizerTypes.Contains(authorizerType))
                    continue;
                
                existingRegistration.AuthorizerTypes.Add(authorizerType);
            }

            if (!string.IsNullOrEmpty(newRegistration.ActivityName) &&
                string.IsNullOrEmpty(existingRegistration.ActivityName))
                existingRegistration.ActivityName = newRegistration.ActivityName;
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

        public void InvokeStaticRegistrationMethods(params Assembly[] assemblies)
        {
            var methods = assemblies.SelectMany(
                a =>
                    a.GetTypes()
                        .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                        .Where(m => m.GetCustomAttributes(typeof (AuthorizationRegistrationAttribute),false).Any()));

            foreach (var methodInfo in methods)
            {
                methodInfo.Invoke(null, new object[] {this});
            }
        }

        public class MultiRegistration
        {
            private readonly AbstractRefereeConfigurationBuilder<TRegistration> _builder;
            public IList<TRegistration> Registrations { get; set; }

            public MultiRegistration(IList<TRegistration> registrations,
                AbstractRefereeConfigurationBuilder<TRegistration> builder)
            {
                _builder = builder;
                Registrations = registrations;
            }

            public void With(Func<TRegistration, TRegistration> acitivyRegistrationExpression)
            {
                foreach (var registration in Registrations)
                {
                    acitivyRegistrationExpression.Invoke(registration);
                    _builder.ValidateAndAddRegistration(registration);

                }
            }
        }
    }
}
