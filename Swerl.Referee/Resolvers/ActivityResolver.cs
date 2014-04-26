using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Swerl.Referee.Activities;
using Swerl.Referee.Configuration;
using Swerl.Referee.Factories;

namespace Swerl.Referee.Resolvers
{
    public class ActivityResolver : IActivityResolver
    {
        private readonly IActivityFactory _activictyFactory;
        private readonly IDictionary<string, Type> _registeredActivityNames;

        private readonly IDictionary<MethodInfo, Type> _registeredActivityMethods; 

        public ActivityResolver(IActivityFactory activictyFactory, IEnumerable<ActivityRegistration> activityRegistrations)
        {
            _activictyFactory = activictyFactory;

            var registrations = activityRegistrations as ActivityRegistration[] ?? activityRegistrations.ToArray();
            
            var activityNamesForActivityResolver = registrations.Where(a=> a.ActivityType != null && a.ActivityMethod == null)
                .ToDictionary(
                    a => !string.IsNullOrEmpty(a.ActivityName) ? a.ActivityName : a.ActivityType.Name,
                    a => a.ActivityType
                );

            _registeredActivityNames = activityNamesForActivityResolver;
            
            _registeredActivityMethods = registrations.Where(
                a => a.ActivityType != null && a.ActivityMethod != null)
                .ToDictionary(a => a.ActivityMethod, a => a.ActivityType);
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
            var method = expression.GetMethodInfor();
            if (_registeredActivityMethods.ContainsKey(method))
                return GetActivity(_registeredActivityMethods[method], expression.GetMethodParams());

            var info = expression.GetMethodInfor();
            var uniqueName = info.Name + "-" + info.DeclaringType.Name;

            return GetActivity(uniqueName);
        }

        public IActivity GetActivity(LambdaExpression expression)
        {
            var method = expression.GetMethodInfor();
            if (_registeredActivityMethods.ContainsKey(method))
                return GetActivity(_registeredActivityMethods[method], expression.GetMethodParams());

            var info = expression.GetMethodInfor();
            var uniqueName = info.Name + "-" + info.DeclaringType.Name;

            return new MethodActivity(method);
        }
    }
}
