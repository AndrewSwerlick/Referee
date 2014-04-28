using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Factories;

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
            var authorizer = (IActivityAuthorizer) DependencyResolver.Current.GetService(T) ??
                             (IActivityAuthorizer)Activator.CreateInstance(T);
            return authorizer;
        }

        public IActivityAuthorizer BuildDefaultAuthorizer()
        {
            return new AllowAnonymous();
        }
    }
}
