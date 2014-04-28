using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Swerl.Referee.Tests.TestClasses
{
    public class TestPrincipal : IPrincipal
    {
        public IList<string> Roles { get; set; } 

        public TestPrincipal()
        {
            Roles = new List<string>();
            Identity = new ClaimsIdentity();
        }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

        public IIdentity Identity { get; private set; }
    }
}
