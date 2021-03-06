﻿using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Swerl.Referee.MVC.Tests.Helpers;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.MVC.Tests.Configuration
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
