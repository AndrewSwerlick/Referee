using System;
using Swerl.Referee.Activities;

namespace Swerl.Referee.Factories
{
    public class ActivityFactory : IActivityFactory
    {
        public IActivity BuildDefaultActivity(string name)
        {
            return new NamedActivity(name);
        }

        public IActivity BuildActivity<T>(params object[] parameters) where T : IActivity
        {
            return BuildActivity(typeof(T), parameters);
        }

        public IActivity BuildActivity(Type type, params object[] parameters)
        {
            return (IActivity)Activator.CreateInstance(type, parameters);
        }
    }
}
