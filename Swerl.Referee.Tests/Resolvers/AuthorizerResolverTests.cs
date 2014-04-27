using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using NUnit.Framework;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Resolvers;
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
                    AuthorizerTypes = new []{typeof(TestAuthorizer)}
                }
            });

            var authorizer = resolver.GetAuthorizers(new NamedActivity("Test Activity")).First();
            Assert.That(authorizer is TestAuthorizer);
        }

        [Test]
        public void Ensure_We_Can_Resolve_An_Authorizer_For_A_Typed_Activity()
        {
            var resolver = BuildAuthorizerResolver(new[]
            {
                new ActivityRegistration
                {
                    ActivityType = typeof(TestActivity), 
                    AuthorizerTypes = new []{typeof(TestAuthorizer)}
                }
            });

            var authorizer = resolver.GetAuthorizers(new TestActivity()).First();
            Assert.That(authorizer is TestAuthorizer);
        }

        [Test]
        public void Ensure_We_Can_Resolve_An_Authorizer_For_A_Method_Activity()
        {
            var resolver = BuildAuthorizerResolver(new[]
            {
                new ActivityRegistration
                {
                    ActivityMethod = typeof(TestCodeClass).GetMethod("DoSomething"), 
                    AuthorizerTypes = new []{typeof(TestAuthorizer)}
                }
            });

            var authorizer =
                resolver.GetAuthorizers(new MethodActivity(typeof (TestCodeClass).GetMethod("DoSomething"))).First();
            Assert.That(authorizer is TestAuthorizer);
        }

        [Test]
        public void
            Ensure_We_Can_Resolve_An_Authorizer_For_A_Method_Activity_When_Two_Methods_With_The_Same_Name_Have_Been_Reigstered
            ()
        {
            var resolver = BuildAuthorizerResolver(new[]
            {
                new ActivityRegistration
                {
                    ActivityMethod = typeof(TestCodeClass).GetMethod("DoSomething"), 
                    AuthorizerTypes = new []{typeof(TestAuthorizer)}
                },
                 new ActivityRegistration
                {
                    ActivityMethod = typeof(TestCodeClass2).GetMethod("DoSomething"), 
                    AuthorizerTypes = new[] {typeof(UnauthorizedAuthorizer)}
                },
            });

            var authorizer1 = resolver.GetAuthorizers(new MethodActivity(typeof(TestCodeClass).GetMethod("DoSomething"))).First();
            var authorizer2 = resolver.GetAuthorizers(new MethodActivity(typeof(TestCodeClass2).GetMethod("DoSomething"))).First();

            Assert.That(authorizer1 is TestAuthorizer);
            Assert.That(authorizer2 is UnauthorizedAuthorizer);
        }

        [Test]
        public void
            Ensure_When_We_Try_To_Resolve_An_Authorizer_For_An_Unregistered_Method_Activity_We_Get_The_Default_Authorizer
            ()
        {
            var resolver = BuildAuthorizerResolver(new ActivityRegistration[] {});
            var authorizer = resolver.GetAuthorizers(new MethodActivity(typeof(TestCodeClass).GetMethod("DoSomething"))).First();

            Assert.That(authorizer is AllowAnonymous);
        }          

        private AuthorizerResolver BuildAuthorizerResolver(IList<ActivityRegistration> registrations)
        {           
            return new AuthorizerResolver(new TestAuthorizerFactory(), registrations);
        }
    }
}
