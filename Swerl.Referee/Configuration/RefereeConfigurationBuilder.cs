using Swerl.Referee.Core.Factories;

namespace Swerl.Referee.Core.Configuration
{
    public class RefereeConfigurationBuilder : AbstractRefereeConfigurationBuilder<ActivityRegistration>
    {
        public RefereeConfigurationBuilder(IAuthorizerFactory authorizerFactory, IActivityFactory activityFactory) : base(authorizerFactory, activityFactory)
        {
        }

        public override ActivityRegistration BuildRegistration()
        {
            return new ActivityRegistration();
        }
    }
}
