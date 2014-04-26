using System.Linq;
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
            conf.Register("Test Activity");

            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(1));
            Assert.That(conf.ActivityRegistrations.First().ActivityName, Is.EqualTo("Test Activity"));
        }

        [Test]
        public void Ensure_We_Can_Register_An_Activity_By_Type()
        {
            var conf = BuildConfigurationObject();
            conf.RegisterActivity<TestActivity>();

            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(1));
            Assert.That(conf.ActivityRegistrations.First().ActivityType, Is.EqualTo(typeof(TestActivity)));
        }

        [Test]
        public void Ensure_We_Can_Register_An_Activity_By_Expression()
        {
            var conf = BuildConfigurationObject();
            conf.Register<TestCodeClass>(c => c.DoSomething(default(string))).As<TestActivity>();

            Assert.That(conf.ActivityRegistrations.Count, Is.EqualTo(1));
            Assert.That(conf.ActivityRegistrations.First().ActivityName, Is.EqualTo("DoSomething"));
            Assert.That(conf.ActivityRegistrations.First().ActivityType, Is.EqualTo(typeof(TestActivity)));
        }

        private RefereeConfigurationBuilder BuildConfigurationObject()
        {
            return new RefereeConfigurationBuilder(new TestAuthorizerFactory(), new ActivityFactory());
        }
    }
}
