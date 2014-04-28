using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Extensions;
using Swerl.Referee.Core.Factories;

namespace Swerl.Referee.Core.Resolvers
{
    public class ActivityResolver : IActivityResolver
    {
        private readonly IActivityFactory _activictyFactory;
        private readonly IEnumerable<IActivityRegistration> _activityRegistrations;
        private readonly IDictionary<string, Type> _registeredActivityNames;

        public ActivityResolver(IActivityFactory activictyFactory, IEnumerable<IActivityRegistration> activityRegistrations)
        {
            _activictyFactory = activictyFactory;
            _activityRegistrations = activityRegistrations;

            var registrations = activityRegistrations as ActivityRegistration[] ?? activityRegistrations.ToArray();
            
            var activityNamesForActivityResolver = registrations.Where(a=> a.ActivityType != null && a.ActivityMethod == null)
                .ToDictionary(
                    a => !string.IsNullOrEmpty(a.ActivityName) ? a.ActivityName : a.ActivityType.Name,
                    a => a.ActivityType
                );

            _registeredActivityNames = activityNamesForActivityResolver;                    
        }

        public IActivity GetActivity(string name)
        {
            if (_registeredActivityNames.ContainsKey(name))
                return GetActivity(_registeredActivityNames[name]);

            return _activictyFactory.BuildDefaultActivity(name);
        }

        public IActivity GetActivity(Type type, params object[] constructorParameters)
        {
            return _activictyFactory.BuildActivity(type, constructorParameters);
        }

        public IActivity GetActivity<T>(Expression<Action<T>> expression)
        {
            return GetActivity((LambdaExpression) expression);
        }

        public IActivity GetActivity(LambdaExpression expression)
        {
            IActivity activity = null;
            var method = expression.GetMethodInfo();            
            var registration = _activityRegistrations.SingleOrDefault(a => a.ActivityMethod == method);
            if (registration != null && registration.ActivityType != null)            
                activity = GetActivity(registration.ActivityType, expression.GetMethodParams());               
            
            if(activity == null)
                activity = new MethodActivity(method);

            activity.Name = !string.IsNullOrEmpty(registration.ActivityName)
                   ? registration.ActivityName
                   : activity.Name;

            return activity;
        }
    }
}
