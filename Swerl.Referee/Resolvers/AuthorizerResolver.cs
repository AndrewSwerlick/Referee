using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IDictionary<Type, IList<Type>> _registeredActivities;
        private readonly IDictionary<string, IList<Type>> _registeredActivityNames;

        public AuthorizerResolver(IAuthorizerFactory authorizerFactory,IEnumerable<IActivityRegistration> activityRegistrations)
        {
            var registrations = activityRegistrations as IActivityRegistration[] ?? activityRegistrations.ToArray();
            var authorizersForTypedActivitiesDictionary =
                registrations.Where(a => a.ActivityType != null).ToDictionary(a => a.ActivityType, a => a.AuthorizerTypes);
            
            var authorizersForNamedActivitiesDictionary = registrations
                .Where(a => a.AuthorizerTypes != null && a.ActivityMethod == null).ToDictionary(
                    a => !string.IsNullOrEmpty(a.ActivityName) ? a.ActivityName : a.ActivityType.Name, 
                    a => a.AuthorizerTypes);            

            var authorizedForMethodActivities = registrations.Where(a => a.ActivityMethod != null).ToDictionary(
                a => a.ActivityMethod.DeclaringType.FullName +"-"+a.ActivityMethod.Name,
                a => a.AuthorizerTypes
                );

            _authorizerFactory = authorizerFactory;
            _registeredActivities = authorizersForTypedActivitiesDictionary;
            _registeredActivityNames = authorizersForNamedActivitiesDictionary;
            foreach (var authorizedForMethodActivity in authorizedForMethodActivities)
            {
                _registeredActivityNames.Add(authorizedForMethodActivity);
            }
        }

        public IList<IActivityAuthorizer> GetAuthorizers(IActivity activity)
        {
            if (_registeredActivities.ContainsKey(activity.GetType()))
                return BuildAuthorizers(_registeredActivities[activity.GetType()]);

            if (_registeredActivityNames.ContainsKey(activity.Name))
                return BuildAuthorizers(_registeredActivityNames[activity.Name]);

            return new[] {_authorizerFactory.BuildDefaultAuthorizer()};
        }

        protected virtual IList<IActivityAuthorizer> BuildAuthorizers(IList<Type> types)
        {
            return types.Select(type=> _authorizerFactory.BuilAuthorizer(type)).ToList();
        }
    }
}
