using Swerl.Referee.Core.Factories;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.MVC.Tests.Helpers
{
    class BuilderHelper
    {
        public static MVCRefereeConfigurationBuilder BuildConfigurationBuilder()
        {
            return new MVCRefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }

        public static MvcAuthorizationService ServiceBuilder(MVCRefereeConfigurationBuilder conf)
        {
            var configuration = conf.Build();

            var service = new MvcAuthorizationService(configuration.AuthorizerResolver, configuration.ActivityResolver,
                new AuthorizationFailureManager(conf.ActivityRegistrations));
            return service;
        }
    }
}
