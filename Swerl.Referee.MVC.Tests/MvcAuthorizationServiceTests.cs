using System.Linq.Expressions;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Swerl.Referee.Activities;
using Swerl.Referee.Authorizers;
using Swerl.Referee.MVC.UnitTests.Helpers;
using Swerl.Referee.MVC.UnitTests.TestClasses;
using Swerl.Referee.Resolvers;
using Swerl.Referee.UnitTests.TestClasses;

namespace Swerl.Referee.MVC.UnitTests
{
    public class MvcAuthorizationServiceTests
    {
        [Test]
        public void Ensure_When_We_Call_The_Authorize_Method_With_An_Unauthorized_ActionExecutingContext_It_Returns_False()
        {
            //arrange
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var authresolver = Mock.Of<IAuthorizerResolver>();
            Mock.Get(authresolver).Setup(r => r.GetAuthorizers(It.IsAny<IActivity>())).Returns(new IActivityAuthorizer[]{new UnauthorizedAuthorizer()});

            var activityResolver = Mock.Of<IActivityResolver>();
            Mock.Get(activityResolver).Setup(s => s.GetActivity(It.IsAny<LambdaExpression>())).Returns(new TestActivity());

            var failureManager = Mock.Of<IAuthorizationFailureManager>();

            var service = new MvcAuthorizationService(authresolver, activityResolver, failureManager);
            //act
            var result = service.Authorize(context, new TestPrincipal());
            
            //assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Ensure_When_We_Call_The_Authorize_Method_With_An_Authorized_ActionExecutingContext_It_Returns_True()
        {
            //arrange
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var authresolver = Mock.Of<IAuthorizerResolver>();
            Mock.Get(authresolver).Setup(r => r.GetAuthorizers(It.IsAny<IActivity>())).Returns(new IActivityAuthorizer[]{new DefaultAuthorizer()});

            var activityResolver = Mock.Of<IActivityResolver>();
            Mock.Get(activityResolver).Setup(s => s.GetActivity(It.IsAny<LambdaExpression>())).Returns(new TestActivity());

            var failureManager = Mock.Of<IAuthorizationFailureManager>();
           
            var service = new MvcAuthorizationService(authresolver, activityResolver, failureManager);
            //act
            var result = service.Authorize(context, new TestPrincipal());

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void
            Ensure_When_We_Call_The_Authorize_Method_And_Handle_Failures_With_An_Unauthorized_ActionExecutingContext_The_Context_Result_Is_Set_To_401
            ()
        {
            //arrange
            var conf = BuilderHelper.BuildConfigurationBuilder();
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            conf.Register(a=> a.Method<TestController>(c=> c.ControllerAction(default(string))).AuthorizedBy<UnauthorizedAuthorizer>());

            var service = BuilderHelper.ServiceBuilder(conf);

            //act
            service.Authorize(context, new TestPrincipal(),true);

            //assert
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(HttpUnauthorizedResult)));
        }

        [Test]
        public void
            Ensure_When_We_Call_The_Authorize_Method_And_Handle_Failures_With_An_Unauthorized_ActionExecutingContext_The_Context_Result_Is_Set_To_The_Registered_Failure_Manager
            ()
        {
            //arrange
            var conf = BuilderHelper.BuildConfigurationBuilder();
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            conf.Register(a => a.Method<TestController>(c => c.ControllerAction(default(string))).AuthorizedBy<UnauthorizedAuthorizer>().HandleFailureWith<HttpNotFoundResult>());

            var service = BuilderHelper.ServiceBuilder(conf);

            //act
            service.Authorize(context, new TestPrincipal(), true);

            //assert
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(HttpNotFoundResult)));
        }

        [Test]
        public void
            Ensure_When_We_Call_The_Authorize_Method_And_Handle_Failures_With_An_Unauthorized_ActionExecutingContext_Registered_By_Method_And_Type_The_Context_Result_Is_Set_To_Registered_Failure_Manager
            ()
        {
            //arrange
            var conf = BuilderHelper.BuildConfigurationBuilder();
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            conf.Register(a => a.Method<TestController>(c => c.ControllerAction(default(string))).As<TestActivity>().AuthorizedBy<UnauthorizedAuthorizer>());

            var service = BuilderHelper.ServiceBuilder(conf);

            //act
            service.Authorize(context, new TestPrincipal(), true);

            //assert
            Assert.That(context.Result.GetType(), Is.EqualTo(typeof(HttpUnauthorizedResult)));
        }
    }
}
