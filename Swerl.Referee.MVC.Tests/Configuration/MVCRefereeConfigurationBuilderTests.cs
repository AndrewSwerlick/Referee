using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using Swerl.Referee.Factories;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.MVC.Factories;
using Swerl.Referee.MVC.UnitTests.Helpers;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.MVC.UnitTests.Configuration
{
    public class MVCRefereeConfigurationBuilderTests
    {
        [Test]
        public void Ensure_We_Can_Register_An_Activity_With_A_Failure_Handler()
        {
            var conf = BuilderHelper.BuildConfigurationBuilder();
            conf.Register(c=> c.As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>().HandleFailureWith<HttpNotFoundResult>());
            Assert.That(conf.ActivityRegistrations.First().FailedResult.GetType(), Is.EqualTo(new HttpNotFoundResult().GetType()));
        }
    }
}
