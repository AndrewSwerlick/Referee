using NUnit.Framework;
using Swerl.Referee.Core;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.UnitTests
{
    public class AuthorizationServiceTests
    {             
        [Test]
        public void Ensure_The_Authorization_Service_Returns_False_For_An_Activity_Instance_Registered_With_The_UnauthorizedAuthorizer()
        {
            var builder = BuildConfigurationBuilder();
            builder.Register(c=> c.As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());
            var service = BuildService(builder.Build());
            var result = service.Authorize(new TestActivity(), new TestPrincipal());
            Assert.That(result, Is.False);
        }

        [Test]
        public void Ensure_The_Authorization_Service_Returns_False_A_Lamdba_Expression_Registered_With_The_Unauthorized_Authorizer()
        {
            var builder = BuildConfigurationBuilder();
            builder.Register(
                a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>());
            var service = BuildService(builder.Build());
            var result = service.Authorize<TestCodeClass>(c => c.DoSomething("test"), new TestPrincipal());
            Assert.That(result, Is.False);
        }

        [Test]
        public void Ensure_The_Authorization_Service_Returns_False_For_Lamdba_Expression_Registered_With_The_Unauthorized_Authorizer_When_More_Than_One_Lambda_Expression_Is_Registered()
        {
            var builder = BuildConfigurationBuilder();
            builder.Register(
                a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>());
            builder.Register(
                a =>
                    a.Method<TestCodeClass2>(c => c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>());
            var service = BuildService(builder.Build());
            var result = service.Authorize<TestCodeClass>(c => c.DoSomething("test"), new TestPrincipal());
            Assert.That(result, Is.False);
        }

        private AuthorizationService BuildService(RefereeConfiguration configuration)
        {
            return new AuthorizationService(configuration.AuthorizerResolver,configuration.ActivityResolver);
        }

        private RefereeConfigurationBuilder BuildConfigurationBuilder()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
