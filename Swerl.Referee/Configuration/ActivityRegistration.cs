using System;
using System.Linq.Expressions;
using System.Reflection;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;

namespace Swerl.Referee.Configuration
{
    public interface IActivityRegistration
    {
        string ActivityName { get; set; }
        MethodInfo ActivityMethod { get; set; }
        Type ActivityType { get; set; }
        Type AuthorizerType { get; set; }
    }

    public class ActivityRegistration<TActivity> : IActivityRegistration where TActivity : ActivityRegistration<TActivity>
    {
        public string ActivityName { get; set; }
        public MethodInfo ActivityMethod { get; set; }
        public Type ActivityType { get; set; }
        public Type AuthorizerType { get; set; }

        public TActivity AuthorizedBy<T>() where T : IActivityAuthorizer
        {
            AuthorizerType = typeof (T);
            return (TActivity)this;
        }

        public TActivity As<T>() where T : IActivity
        {
            ActivityType = typeof (T);
            return (TActivity)this;
        }

        public TActivity Name(string name)
        {
            ActivityName = name;
            return (TActivity)this;
        }

        public TActivity Method<T>(Expression<Action<T>> expression)
        {
            var method = (MethodCallExpression) expression.Body;
            ActivityMethod = method.Method;
            return (TActivity) this;
        }
    }

    public class ActivityRegistration : ActivityRegistration<ActivityRegistration>
    {
        
    }
}