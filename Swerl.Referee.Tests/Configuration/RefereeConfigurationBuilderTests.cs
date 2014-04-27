using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Factories;
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
        public void Ensure_That_If_We_Try_To_Register_The_Same_Method_Twice_With_Different_Types_It_Throws_An_Exception()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            Assert.Throws<InvalidRegistrationException>(()=>conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity2>().AuthorizedBy<UnauthorizedAuthorizer>()));
        }


        [Test]
        public void Ensure_That_If_We_Try_To_Register_The_Same_Name_Twice_It_Combines_The_Authorize_Types()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Name("Test").AuthorizedBy<UnauthorizedAuthorizer>());
            conf.Register(a => a.Name("Test").AuthorizedBy<AllowAnonymous>());
            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(1));
            Assert.That(conf.ActivityRegistrations.First().AuthorizerTypes.Count, Is.EqualTo(2));
        }

        [Test]
        public void Ensure_That_If_We_Try_To_Register_The_Same_Name_Twice_With_Duplicate_Authorizer_Types_The_Authorizer_Types_List_Contains_One_Items()
        {
            var conf = BuildConfigurationObject();
            conf.Register(a => a.Name("Test").AuthorizedBy<UnauthorizedAuthorizer>());
            conf.Register(a => a.Name("Test").AuthorizedBy<AllowAnonymous>());
            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(1));
            Assert.That(conf.ActivityRegistrations.First().AuthorizerTypes.Count, Is.EqualTo(2));
        }

        [Test]
        public void Ensure_We_Can_Register_All_Public_Methods_In_A_Class_As_Activities()
        {
            var conf = BuildConfigurationObject();
            conf.RegisterClassMethods<TestCodeClass>(a=> a.AuthorizedBy<UnauthorizedAuthorizer>());

            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(2));
            Assert.That(conf.ActivityRegistrations.Any(r=> r.ActivityMethod.Name== "DoSomething"));
            Assert.That(conf.ActivityRegistrations.Any(r => r.ActivityMethod.Name == "DoSomething2"));
        }

        [Test]
        public void
            Ensure_That_if_We_Try_To_Set_The_Method_Of_Our_Registrations_When_Registering_A_Class_It_Throws_An_Exception
            ()
        {
            var conf = BuildConfigurationObject();
            Assert.Throws<InvalidRegistrationException>(()=> conf.RegisterClassMethods<TestCodeClass>(a => a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>()));
        }

        [Test]
        public void Ensure_That_We_Can_Register_A_Method_Both_Through_Class_Registration_And_Individually()
        {
            var conf = BuildConfigurationObject();
            conf.RegisterClassMethods<TestCodeClass>(a=> a.AuthorizedBy<UnauthorizedAuthorizer>());
            conf.Register(a=> a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).AuthorizedBy<AllowAnonymous>());
            
            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(2));
            Assert.That(conf.ActivityRegistrations.Single(a=> a.ActivityMethod.Name == "DoSomething").AuthorizerTypes.Count, Is.EqualTo(2));
        }

        [Test]
        public void Ensure_We_Can_Call_Registration_Methods_Defined_In_Other_Classes_With_Attributes()
        {
            var conf = BuildConfigurationObject();
            conf.InvokeStaticRegistrationMethods(typeof (RefereeConfigurationBuilderTests).Assembly);

            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(1));
        }

        private RefereeConfigurationBuilder BuildConfigurationObject()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
