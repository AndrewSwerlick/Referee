using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.Core.Configuration
{
    public interface IActivityRegistration
    {
        string ActivityName { get; set; }
        MethodInfo ActivityMethod { get; set; }
        Type ActivityType { get; set; }
        IList<Type> AuthorizerTypes { get; set; }
    }

    public class ActivityRegistration<TActivity> : IActivityRegistration where TActivity : ActivityRegistration<TActivity>
    {
        public string ActivityName { get; set; }
        public MethodInfo ActivityMethod { get; set; }
        public Type ActivityType { get; set; }
        public IList<Type> AuthorizerTypes { get; set; }

        public ActivityRegistration()
        {
            AuthorizerTypes = new List<Type>();
        }
          

        public TActivity AuthorizedBy<T>() where T : IActivityAuthorizer
        {
            AuthorizerTypes.Add(typeof (T));
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