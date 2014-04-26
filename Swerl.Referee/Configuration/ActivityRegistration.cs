using System;
using System.Reflection;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;

namespace Swerl.Referee.Configuration
{
    public class ActivityRegistration
    {
        public string ActivityName { get; set; }
        public MethodInfo ActivityMethod { get; set; }
        public Type ActivityType { get; set; }
        public Type AuthorizerType { get; set; }

        public ActivityRegistration AuthorizedBy<T>() where T : IActivityAuthorizer
        {
            AuthorizerType = typeof (T);
            return this;
        }

        public ActivityRegistration As<T>() where T : IActivity
        {
            ActivityType = typeof (T);
            return this;
        }
    }
}