using System.Linq.Expressions;
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
        public void Ensure_When_We_Call_The_Authorize_Method_With_An_Unauthorized_ActionExecuting_Context_It_Returns_False()
        {
            //arrange
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var authresolver = Mock.Of<IAuthorizerResolver>();
            Mock.Get(authresolver).Setup(r => r.GetAuthorizer(It.IsAny<IActivity>())).Returns(new UnauthorizedAuthorizer());

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
        public void Ensure_When_We_Call_The_Authorize_Method_With_A_Valid_Authorization_Scenario_It_Returns_True()
        {
            //arrange
            var context = FilterContextHelper.ContextFromExpression<TestController>(c => c.ControllerAction("test"));

            var authresolver = Mock.Of<IAuthorizerResolver>();
            Mock.Get(authresolver).Setup(r => r.GetAuthorizer(It.IsAny<IActivity>())).Returns(new DefaultAuthorizer());

            var activityResolver = Mock.Of<IActivityResolver>();
            Mock.Get(activityResolver).Setup(s => s.GetActivity(It.IsAny<LambdaExpression>())).Returns(new TestActivity());

            var failureManager = Mock.Of<IAuthorizationFailureManager>();
           
            var service = new MvcAuthorizationService(authresolver, activityResolver, failureManager);
            //act
            var result = service.Authorize(context, new TestPrincipal());

            //assert
            Assert.That(result, Is.True);
        }       
    }
}
