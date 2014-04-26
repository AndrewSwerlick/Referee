using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Authorizers;

namespace Swerl.Referee.Factories
{
    public interface IAuthorizerFactory
    {
        IActivityAuthorizer BuildAuthorizer<T>() where T : IActivityAuthorizer;
        IActivityAuthorizer BuilAuthorizer(Type T);
        IActivityAuthorizer BuildDefaultAuthorizer();
    }
}
