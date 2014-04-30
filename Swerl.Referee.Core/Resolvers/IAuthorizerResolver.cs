using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.Core.Resolvers
{
    public interface IAuthorizerResolver
    {
        IList<IActivityAuthorizer> GetAuthorizers(IActivity activity);
        IList<IActivityAuthorizer> GetAuthorizers<T>(Expression<Action<T>> expression);
        IList<IActivityAuthorizer> GetAuthorizers(LambdaExpression expression);
    }
}