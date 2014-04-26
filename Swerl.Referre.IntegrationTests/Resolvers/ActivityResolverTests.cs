using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Swerl.Referee.Configuration;
using Swerl.Referee.Factories;
using Swerl.Referee.Resolvers;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referre.IntegrationTests.Resolvers
{
    class ActivityResolverTests
    {
        [Test]
        public void Ensure_We_Can_Resolve_An_Activity_By_Expression_With_A_Complex_Expression()
        {
            var conf = BuildConfigurationObject();
            conf.Register<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>();

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var test = "test";
            var activity = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething("te" + "st"));
            Assert.That(activity.Id, Is.EqualTo("test"));
        }

        [Test]
        public void
            Ensure_When_We_Register_Two_Methods_With_The_Same_Names_As_Different_Activities_We_Can_Resolve_Each_Individually
            ()
        {
            var conf = BuildConfigurationObject();
            conf.Register<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>();
            conf.Register<TestCodeClass2>(c => c.DoSomething(default(string))).As<TestActivity2>();

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var act1 = resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));
            var act2 = resolver.GetActivity<TestCodeClass2>(c => c.DoSomething("test"));

            Assert.That(act1 is TestActivity);
            Assert.That(act2 is TestActivity2);
        }

         [Test]
        public void Ensure_We_Can_Resolve_An_Activity_By_Expression_With_A_Variable()
        {
            var conf = BuildConfigurationObject();
            conf.Register<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>();

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var test = "test";
            var activity = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething(test));
            Assert.That(activity.Id, Is.EqualTo("test"));
        }

       
        protected ActivityResolver BuildActivityResolver(IEnumerable<ActivityRegistration> registrations)
        {
            return new ActivityResolver(new ActivityFactory(), registrations);
        }

        protected RefereeConfigurationBuilder BuildConfigurationObject()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }    
}
