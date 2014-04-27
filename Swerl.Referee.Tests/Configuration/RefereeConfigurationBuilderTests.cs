using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Swerl.Referee.Configuration;
using Swerl.Referee.Factories;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.UnitTests.Configuration
{
    class RefereeConfigurationBuilderTests
    {
        [Test]
        public void Ensure_We_Can_Register_An_Activity_By_Name()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Name("TestActivity").AuthorizedBy<UnauthorizedAuthorizer>());
            Assert.That(conf.ActivityRegistrations.First().ActivityName, Is.EqualTo("TestActivity"));
        }

        [Test]
        public void Ensure_We_Can_Register_An_Activity_By_Type()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());
            Assert.That(conf.ActivityRegistrations.First().ActivityType, Is.EqualTo(typeof(TestActivity)));
        }

        [Test]
        public void Ensure_We_Can_Register_An_Activity_By_Expression()
        {
            var conf = BuildConfigurationObject();
            Assert.Throws<InvalidRegistrationException>(() => conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>()));
        }

        [Test]
        public void Ensure_That_If_We_Try_To_Register_An_Activity_Without_An_Authorizer_It_Throws_An_Exception()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());
        }

        [Test]
        public void Ensure_That_If_We_Try_To_Register_The_Same_Method_Twice_It_Throws_An_Exception()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            Assert.Throws<InvalidRegistrationException>(()=>conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>()));
        }

        [Test]
        public void Ensure_That_If_We_Try_To_Register_The_Same_Type_Twice_Without_Method_Information_It_Throws_An_Exception()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            Assert.Throws<InvalidRegistrationException>(() => conf.Register(a => a.As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>()));
        }

        [Test]
        public void Ensure_That_If_We_Try_To_Register_The_Same_Name_Twice_Without_Method_Information_It_Throws_An_Exception()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Name("Test").AuthorizedBy<UnauthorizedAuthorizer>());

            Assert.Throws<InvalidRegistrationException>(() => conf.Register(a => a.Name("Test").AuthorizedBy<UnauthorizedAuthorizer>()));
        }

        private RefereeConfigurationBuilder BuildConfigurationObject()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
