using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.Tests.Helpers
{
    class BuilderHelper
    {
        public static RefereeConfigurationBuilder BuildConfigurationObject()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
