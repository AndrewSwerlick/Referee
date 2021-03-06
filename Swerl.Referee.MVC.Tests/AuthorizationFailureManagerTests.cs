﻿using System.Web.Mvc;
using NUnit.Framework;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.MVC.Tests.Helpers;
using Swerl.Referee.MVC.Tests.TestClasses;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.MVC.Tests
{
    public class AuthorizationFailureManagerTests
    {
        [Test]
        public void Ensure_That_The_Failure_Manager_Assigns_A_404_Result_To_An_Unauthorizer_Activity_Registered_By_Type_With_A_404_Handler()
        {
            var conf = BuilderHelper.BuildConfigurationBuilder();
            conf.Register(c=> c.As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>().HandleFailureWith<HttpNotFoundResult>());

            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var manager = new AuthorizationFailureManager(conf.ActivityRegistrations);
            manager.HandleFailedAuthorization(new TestActivity(), context);
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(HttpNotFoundResult)));
        }

        [Test]
        public void Ensure_That_The_Failure_Manager_Assigns_A_404_Result_To_An_Unauthorizer_Activity_Registered_By_Method_With_A_404_Handler()
        {
            var conf = BuilderHelper.BuildConfigurationBuilder();
            conf.Register(a => a.Method<TestCodeClass>(c=> c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>().HandleFailureWith<HttpNotFoundResult>());

            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var manager = new AuthorizationFailureManager(conf.ActivityRegistrations);
            manager.HandleFailedAuthorization(new MethodActivity(typeof(TestCodeClass).GetMethod("DoSomething")), context);
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(HttpNotFoundResult)));
        }

        [Test]
        public void Ensure_That_The_Failure_Manager_Assigns_A_404_Result_To_An_Unauthorizer_Activity_Registered_By_Name_With_A_404_Handler()
        {
            var conf = BuilderHelper.BuildConfigurationBuilder();
            conf.Register(a => a.Name("Test").AuthorizedBy<UnauthorizedAuthorizer>().HandleFailureWith<HttpNotFoundResult>());

            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var manager = new AuthorizationFailureManager(conf.ActivityRegistrations);
            manager.HandleFailedAuthorization(new NamedActivity("Test"), context);
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(HttpNotFoundResult)));
        }

        
    }
}
