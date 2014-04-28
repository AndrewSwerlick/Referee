using System.Collections.Generic;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.Core.Resolvers
{
    public interface IAuthorizerResolver
    {
        IList<IActivityAuthorizer> GetAuthorizers(IActivity activity);
    }
}