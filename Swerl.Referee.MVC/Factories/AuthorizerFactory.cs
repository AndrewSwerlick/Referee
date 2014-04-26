using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Authorizers;
using Swerl.Referee.Factories;

namespace Swerl.Referee.MVC.Factories
{
    public class AuthorizerFactory :IAuthorizerFactory
    {
        public IActivityAuthorizer BuildAuthorizer<T>() where T : IActivityAuthorizer
        {
            return BuilAuthorizer(typeof (T));
        }

        public IActivityAuthorizer BuilAuthorizer(Type T)
        {
            return (IActivityAuthorizer) DependencyResolver.Current.GetService(T);
        }

        public IActivityAuthorizer BuildDefaultAuthorizer()
        {
            return new DefaultAuthorizer();
        }
    }
}
