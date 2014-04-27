using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.MVC.UnitTests.Helpers
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
