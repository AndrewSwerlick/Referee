using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;
using Swerl.Referee.Configuration;
using Swerl.Referee.Factories;

namespace Swerl.Referee.Resolvers
{
    public class AuthorizerResolver : IAuthorizerResolver
    {
        private readonly IAuthorizerFactory _authorizerFactory;
        private readonly IDictionary<Type, Type> _registeredActivities;
        private readonly IDictionary<string, Type> _registeredActivityNames;

        public AuthorizerResolver(IAuthorizerFactory authorizerFactory,IList<ActivityRegistration> activityRegistrations)
        {
            var authorizersForTypedActivitiesDictionary =
                activityRegistrations.Where(a => a.AuthorizerType != null && a.ActivityType != null).ToDictionary(a => a.ActivityType, a => a.AuthorizerType);
            
            var authorizersForNamedActivitiesDictionary = activityRegistrations
                .Where(a => a.AuthorizerType != null && a.ActivityMethod == null).ToDictionary(
                    a => !string.IsNullOrEmpty(a.ActivityName) ? a.ActivityName : a.ActivityType.Name, 
                    a => a.AuthorizerType);            

            var authorizedForMethodActivities = activityRegistrations.Where(a => a.ActivityMethod != null).ToDictionary(
                a => a.ActivityMethod.DeclaringType.FullName +"-"+a.ActivityMethod.Name,
                a => a.AuthorizerType
                );

            _authorizerFactory = authorizerFactory;
            _registeredActivities = authorizersForTypedActivitiesDictionary;
            _registeredActivityNames = authorizersForNamedActivitiesDictionary;
            foreach (var authorizedForMethodActivity in authorizedForMethodActivities)
            {
                _registeredActivityNames.Add(authorizedForMethodActivity);
            }
        }

        public IActivityAuthorizer GetAuthorizer(IActivity activity)
        {
            if (_registeredActivities.ContainsKey(activity.GetType()))
                return BuildAuthorizer(_registeredActivities[activity.GetType()]);

            if (_registeredActivityNames.ContainsKey(activity.Name))
                return BuildAuthorizer(_registeredActivityNames[activity.Name]);

            return _authorizerFactory.BuildDefaultAuthorizer();
        }

        protected virtual IActivityAuthorizer BuildAuthorizer(Type type)
        {
            return _authorizerFactory.BuilAuthorizer(type);
        }
    }
}
