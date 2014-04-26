using System.Collections.Generic;
using NUnit.Framework;
using Swerl.Referee.Configuration;
using Swerl.Referee.Factories;
using Swerl.Referee.Resolvers;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.UnitTests.Resolvers
{
    public class ActivityResolverTests
    {       
        [Test]
        public void Ensure_We_Can_Resolve_A_Named_Activity_By_Name()
        {
            var config = BuildConfigurationObject();

            var resolver = BuildActivityResolver(new List<ActivityRegistration>(){new ActivityRegistration
            {
                ActivityName = "Activity"
            }});
            var activity = resolver.GetActivity("Activity");
            Assert.That(activity.Name, Is.EqualTo("Activity"));
        }

        [Test]
        public void Ensure_We_Can_Resolve_A_Typed_Activity_By_Type()
        {
            var resolver = BuildActivityResolver(new List<ActivityRegistration>
            {
                new ActivityRegistration()
                {
                    ActivityType = typeof (TestActivity),
                }
            });

            var activity = resolver.GetActivity(typeof (TestActivity));
            Assert.That(activity, Is.Not.Null);
            Assert.That(activity is TestActivity);
        }

        [Test]
        public void Ensure_We_Can_Resolve_A_Typed_Activity_By_Type_With_Constructor_Params()
        {
            var resolver = BuildActivityResolver(new List<ActivityRegistration>
            {
                new ActivityRegistration()
                {
                    ActivityType = typeof (TestActivity),
                }
            });

            var activity = (TestActivity)resolver.GetActivity(typeof (TestActivity), new object[] {"Test"});
            Assert.That(activity.Id, Is.EqualTo("Test"));
        }

        [Test]
        public void Ensure_We_Can_Resolve_A_Typed_Activity_By_Name()
        {
            var resolver = BuildActivityResolver(new List<ActivityRegistration>
            {
                new ActivityRegistration()
                {
                    ActivityType = typeof (TestActivity),
                }
            });

            var activity = resolver.GetActivity("TestActivity");
            Assert.That(activity is TestActivity);
        }        

        [Test]
        public void Ensure_We_Can_Resolve_An_Activity_By_Expression()
        {
            var conf = BuildConfigurationObject();
            conf.Register<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>();

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);

            var activity = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));
            Assert.That(activity.Id, Is.EqualTo("test"));

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

        [Test]
        public void Ensure_We_Can_Resolve_An_Activity_By_Expression_With_A_Complex_Expression()
        {
            var conf = BuildConfigurationObject();
            conf.Register<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>();

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var test = "test";
            var activity = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething("te"+"st"));
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
        public void Ensure_We_Can_Register_An_Activity_By_Name_And_Resolve_It_By_Expression()
        {
            //arrange
            var conf = BuildConfigurationObject();
            conf.Register("DoSomething");

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var activity = resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));

            Assert.That(activity.Name, Is.EqualTo("DoSomething-TestCodeClass"));
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
