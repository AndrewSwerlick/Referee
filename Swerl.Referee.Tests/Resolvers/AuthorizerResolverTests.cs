using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using NUnit.Framework;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Resolvers;
using Swerl.Referee.Tests.Helpers;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.Tests.Resolvers
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
                    AuthorizerTypes = new Dictionary<Type, Action<IActivityAuthorizer>>{{typeof(TestAuthorizer),null}}.ToList()
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
                    AuthorizerTypes = new Dictionary<Type, Action<IActivityAuthorizer>>{{typeof(TestAuthorizer),null}}.ToList()
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
                    AuthorizerTypes = new Dictionary<Type, Action<IActivityAuthorizer>>{{typeof(TestAuthorizer),null}}.ToList()
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
                    AuthorizerTypes = new Dictionary<Type, Action<IActivityAuthorizer>>{{typeof(TestAuthorizer),null}}.ToList()
                },
                 new ActivityRegistration
                {
                    ActivityMethod = typeof(TestCodeClass2).GetMethod("DoSomething"), 
                    AuthorizerTypes = new Dictionary<Type, Action<IActivityAuthorizer>>{{typeof(UnauthorizedAuthorizer),null}}.ToList()
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

        [Test]
        public void
            Ensure_When_We_Configure_An_Authorizer_With_A_Post_Build_Action_The_Resolver_Returns_An_Authorizer_That_Has_Had_The_Action_Executed_Against_It
            ()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a=> a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).AuthorizedBy<HasRoles>(r=> r.AuthorizedRoles = new[] {"Test"}));

            var resolver = BuildAuthorizerResolver(conf.ActivityRegistrations);
            var authorizer = (HasRoles)resolver.GetAuthorizers(new MethodActivity(typeof (TestCodeClass).GetMethod("DoSomething"))).First();
            Assert.That(authorizer.AuthorizedRoles.First(), Is.EqualTo("Test"));
        }

        [Test]
        public void Ensure_When_We_Register_A_Method_Activity_By_Name_We_Can_Resolve_Is_Authorizer()
        {            
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a=> a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).Name("TestActivity").AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildAuthorizerResolver(conf.ActivityRegistrations);
            var authorizer = resolver.GetAuthorizers(new MethodActivity(typeof (TestCodeClass).GetMethod("DoSomething"))
            {
                Name = "TestActivity"
            }).First();

            Assert.That(authorizer is UnauthorizedAuthorizer);
        }

        [Test]
        public void
            Ensure_That_If_A_Method_Activity_Has_Been_Registered_With_The_Allow_Anonymous_Authorizer_That_Is_The_Only_One_The_Authorizer_Resolver_Returns
            ()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>());
            conf.Register(a=> a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).AuthorizedBy<AllowAnonymous>());

            var resolver = BuildAuthorizerResolver(conf.ActivityRegistrations);
            var authorizers = resolver.GetAuthorizers(new MethodActivity(typeof(TestCodeClass).GetMethod("DoSomething")));

            Assert.That(authorizers.Count, Is.EqualTo(1));
        }

        private AuthorizerResolver BuildAuthorizerResolver(IList<ActivityRegistration> registrations)
        {           
            return new AuthorizerResolver(new TestAuthorizerFactory(), registrations);
        }
    }
}
