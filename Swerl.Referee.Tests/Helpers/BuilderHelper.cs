using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.UnitTests.Helpers
{
    class BuilderHelper
    {
        public static RefereeConfigurationBuilder BuildConfigurationObject()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
