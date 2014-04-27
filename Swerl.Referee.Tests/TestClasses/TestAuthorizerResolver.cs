using System;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Factories;

namespace Swerl.Referee.UnitTests.TestClasses
{
    public class TestAuthorizerFactory : IAuthorizerFactory
    {        
        public IActivityAuthorizer BuildAuthorizer<T>() where T : IActivityAuthorizer
        {
            return Activator.CreateInstance<T>();
        }

        public IActivityAuthorizer BuilAuthorizer(Type T)
        {
            return (IActivityAuthorizer) Activator.CreateInstance(T);
        }

        public IActivityAuthorizer BuildDefaultAuthorizer()
        {
            return new DefaultAuthorizer();
        }
    }
}
