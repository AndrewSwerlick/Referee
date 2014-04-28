using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Moq;
using NUnit.Framework;
using Swerl.Referee.MVC.Tests.TestClasses;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.MVC.Tests
{
    class AuthorizeActivityFilterTests
    {
        [Test]
        public void
            Ensure_That_When_An_Unauthorized_User_Executes_The_AuthorizeActivity_Filter_For_An_Activity_Registered_By_Expression_The_ActionContext_Result_Is_Set_To_A_401()
        {
            var httpcontext = Mock.Of<HttpContextBase>();
            Mock.Get(httpcontext).Setup(x => x.User).Returns(new TestPrincipal());

            var context = new ActionExecutingContext(new ControllerContext
            {
                Controller = new TestController(),
                HttpContext = httpcontext
            },
                new ReflectedActionDescriptor(typeof (TestController).GetMethod("ControllerAction"), "ControllerAction",
                    new ReflectedAsyncControllerDescriptor(typeof (TestController))),
                new Dictionary<string, object> {{"id", "test"}});

            var service = Mock.Of<IMvcAuthorizationService>();
            Mock.Get(service)
                .Setup(s => s.Authorize(context, It.IsAny<IPrincipal>(),true))
                .Callback(() =>
                {
                    context.Result = new HttpUnauthorizedResult();
                })
                .Returns(false);

            var filters = new GlobalFilterCollection();
            var filter = new AuthorizeActivity(service, filters);
            filters.Add(filter);

            filter.OnActionExecuting(context);

            Assert.That(context.Result is HttpUnauthorizedResult);
        }
    }
}
