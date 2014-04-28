using System;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.Core.Factories
{
    public interface IAuthorizerFactory
    {
        IActivityAuthorizer BuildAuthorizer<T>() where T : IActivityAuthorizer;
        IActivityAuthorizer BuilAuthorizer(Type T);
        IActivityAuthorizer BuildDefaultAuthorizer();
    }
}
