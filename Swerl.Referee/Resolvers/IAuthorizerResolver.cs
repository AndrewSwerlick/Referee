using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;

namespace Swerl.Referee.Resolvers
{
    public interface IAuthorizerResolver
    {
        IActivityAuthorizer GetAuthorizer(IActivity activity);
    }
}