﻿using System.Collections.Generic;
using System.Reflection.Emit;
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
        public void Ensure_We_Can_Resolve_An_Authorizer_For_A_Typed_Activity()
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

        [Test]
        public void Ensure_We_Can_Resolve_An_Authorizer_For_A_Method_Activity()
        {
            var resolver = BuildAuthorizerResolver(new[]
            {
                new ActivityRegistration
                {
                    ActivityMethod = typeof(TestCodeClass).GetMethod("DoSomething"), 
                    AuthorizerType = typeof(TestAuthorizer)
                }
            });

            var authorizer = resolver.GetAuthorizer(new MethodActivity(typeof (TestCodeClass).GetMethod("DoSomething")));
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
                    AuthorizerType = typeof(TestAuthorizer)
                },
                 new ActivityRegistration
                {
                    ActivityMethod = typeof(TestCodeClass2).GetMethod("DoSomething"), 
                    AuthorizerType = typeof(UnauthorizedAuthorizer)
                },
            });

            var authorizer1 = resolver.GetAuthorizer(new MethodActivity(typeof(TestCodeClass).GetMethod("DoSomething")));
            var authorizer2 = resolver.GetAuthorizer(new MethodActivity(typeof(TestCodeClass2).GetMethod("DoSomething")));

            Assert.That(authorizer1 is TestAuthorizer);
            Assert.That(authorizer2 is UnauthorizedAuthorizer);
        }

        [Test]
        public void
            Ensure_When_We_Try_To_Resolve_An_Authorizer_For_An_Unregistered_Method_Activity_We_Get_The_Default_Authorizer
            ()
        {
            var resolver = BuildAuthorizerResolver(new ActivityRegistration[]{});
            var authorizer = resolver.GetAuthorizer(new MethodActivity(typeof(TestCodeClass).GetMethod("DoSomething")));

            Assert.That(authorizer is DefaultAuthorizer);
        }

        private AuthorizerResolver BuildAuthorizerResolver(IList<ActivityRegistration> registrations)
        {           
            return new AuthorizerResolver(new TestAuthorizerFactory(), registrations);
        }
    }
}
