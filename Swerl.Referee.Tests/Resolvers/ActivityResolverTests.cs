using System.Collections.Generic;
using NUnit.Framework;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.Core.Resolvers;
using Swerl.Referee.Tests.Helpers;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.Tests.Resolvers
{
    public class ActivityResolverTests
    {      
        [Test]
        public void Ensure_We_Can_Resolve_A_Named_Activity_By_Name()
        {
            var config = BuilderHelper.BuildConfigurationObject();

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
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);

            var activity = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));
            Assert.That(activity.Id, Is.EqualTo("test"));

        }

        [Test]
        public void Ensure_We_Can_Resolve_An_Activity_By_Expression_With_A_Variable()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var test = "test";
            var activity = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething(test));
            Assert.That(activity.Id, Is.EqualTo("test"));
        }

        [Test]
        public void Ensure_We_Can_Resolve_An_Activity_By_Expression_With_A_Complex_Expression()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

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
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());
            conf.Register(a => a.Method<TestCodeClass2>(c => c.DoSomething(default(string))).As<TestActivity2>().AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var act1 = resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));
            var act2 = resolver.GetActivity<TestCodeClass2>(c => c.DoSomething("test"));

            Assert.That(act1 is TestActivity);
            Assert.That(act2 is TestActivity2);
        }

        [Test]
        public void
            Ensure_When_We_Register_A_Method_As_A_Specific_Activity_Type_Ensure_When_We_Resolve_By_Lamdba_Expression_The_Method_Parameters_Are_Used_In_The_Type_Constructor
            ()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var act = (TestActivity)resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));

            Assert.That(act.Id, Is.EqualTo("test"));
        }

        [Test]
        public void
            Ensure_When_We_Register_Two_Methods_With_The_Same_As_The_Same_Activity_Type_We_Can_Resolve_Each_Individually
            ()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());
            conf.Register(a => a.Method<TestCodeClass2>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var act1 = resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));
            var act2 = resolver.GetActivity<TestCodeClass2>(c => c.DoSomething("test"));

            Assert.That(act1 is TestActivity);
            Assert.That(act2 is TestActivity);
        }

        [Test]
        public void
            Ensure_When_We_Register_The_Method_Of_A_Base_Class_We_Can_Resolve_The_Activity_With_An_Expression_Using_The_Child_Class
            ()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a => a.Method<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var act = resolver.GetActivity<TestChildClass>(c => c.DoSomething("test"));

            Assert.That(act is TestActivity);
        }


        [Test]
        public void
            Ensure_If_We_Provide_A_Specific_Name_For_An_Activity_Registered_By_Method_The_Name_Is_Correctly_Set_On_The_Resolved_Activity
            ()
        {
            var conf = BuilderHelper.BuildConfigurationObject();
            conf.Register(a=> a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).Name("TestActivity").AuthorizedBy<AllowAnonymous>());

            var resolver = BuildActivityResolver(conf.ActivityRegistrations);
            var activity = resolver.GetActivity<TestCodeClass>(c => c.DoSomething("test"));
            
            Assert.That(activity.Name, Is.EqualTo("TestActivity"));
        }

        protected ActivityResolver BuildActivityResolver(IEnumerable<ActivityRegistration> registrations)
        {
            return new ActivityResolver(new ActivityFactory(), registrations);
        }       
    }
}
