using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Factories;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.MVC.UnitTests.Helpers
{
    class ConfigurationBuilderHelper
    {
        public static MVCRefereeConfigurationBuilder BuildConfigurationBuilder()
        {
            return new MVCRefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
