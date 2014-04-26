using System.Collections.Generic;
using System.Security.Principal;
using NUnit.Framework;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;
using Swerl.Referee.Configuration;
using Swerl.Referee.Resolvers;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.UnitTests.Resolvers
{
    public class AuthorizerResolverTests
    {
        public class TestAuthorizer : IActivityAuthorizer
        {
            public bool Authorize(IActivity activity, IPrincipal user)
            {
                return true;
            }
        }

        [Test]
        public void Ensure_We_Can_Resolve_An_Authorizer_For_A_Named_Activity()
        {
            var resolver = BuildAuthorizerResolver(new[]
            {
                new ActivityRegistration
                {
                    ActivityName = "Test Activity", 
                    AuthorizerType = typeof(TestAuthorizer)
                }
            });

            var authorizer = resolver.GetAuthorizer(new NamedActivity("Test Activity"));
            Assert.That(authorizer is TestAuthorizer);
        }

        [Test]
        public void Ensure_We_Resolve_An_Authorizer_For_A_Typed_Activity()
        {
            var resolver = BuildAuthorizerResolver(new[]
            {
                new ActivityRegistration
                {
                    ActivityType = typeof(TestActivity), 
                    AuthorizerType = typeof(TestAuthorizer)
                }
            });

            var authorizer = resolver.GetAuthorizer(new TestActivity());
            Assert.That(authorizer is TestAuthorizer);
        }

        private AuthorizerResolver BuildAuthorizerResolver(IList<ActivityRegistration> registrations)
        {           
            return new AuthorizerResolver(new TestAuthorizerFactory(), registrations);
        }
    }
}
