using System.Collections.Generic;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;

namespace Swerl.Referee.Resolvers
{
    public interface IAuthorizerResolver
    {
        IList<IActivityAuthorizer> GetAuthorizers(IActivity activity);
    }
}