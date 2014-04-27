using Swerl.Referee.Core.Resolvers;

namespace Swerl.Referee.Core.Configuration
{
    public class RefereeConfiguration
    {
        public RefereeConfiguration(IActivityResolver activityResolver, IAuthorizerResolver authorizerResolver)
        {
            ActivityResolver = activityResolver;
            AuthorizerResolver = authorizerResolver;
        }

        public IActivityResolver ActivityResolver { get; private set; }
        public IAuthorizerResolver AuthorizerResolver { get; private set; }
    }
}