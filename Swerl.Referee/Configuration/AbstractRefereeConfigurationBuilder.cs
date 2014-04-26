using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Swerl.Referee.Activities;
using Swerl.Referee.Factories;
using Swerl.Referee.Resolvers;

namespace Swerl.Referee.Configuration
{
    public abstract class AbstractRefereeConfigurationBuilder<TRegistration> where TRegistration : ActivityRegistration
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

        public TRegistration Register(string activityName)
        {
            var builder = BuildRegistration();
            builder.ActivityName = activityName;
            ActivityRegistrations.Add(builder);
            return builder;
        }

        public TRegistration RegisterActivity<T>() where T : IActivity
        {
            var builder = BuildRegistration();
            builder.ActivityType = typeof (T);
            builder.ActivityName = typeof (T).Name;
            ActivityRegistrations.Add(builder);
            return builder;
        }

        public RefereeConfiguration Build()
        {
            var authorizerResolver = new AuthorizerResolver(_authorizerFactory, ActivityRegistrations.Cast<ActivityRegistration>().ToList());
            var activityResolver = new ActivityResolver(_activityFactory, ActivityRegistrations);

            return new RefereeConfiguration(activityResolver, authorizerResolver);
        }

        public TRegistration Register<TModel>(Expression<Action<TModel>> expression)
        {           
            var builder = BuildRegistration();
            builder.ActivityName = expression.GetMethodName();
            builder.ActivityMethod = expression.GetMethodInfor();
            ActivityRegistrations.Add(builder);
            return builder;
        } 


        public abstract TRegistration BuildRegistration();
    }
}
