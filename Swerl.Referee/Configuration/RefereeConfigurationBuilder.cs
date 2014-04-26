using Swerl.Referee.Factories;

namespace Swerl.Referee.Configuration
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
