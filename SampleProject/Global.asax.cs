using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Swerl.Referee.Core.Authorizers;

namespace Swerl.Referee.NerdDinnerSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MVC.Referee.Configure((builder) =>
            {
                builder.Register(c=> c.Name("Test").AuthorizedBy<DefaultAuthorizer>().HandleFailureWith<HttpNotFoundResult>());
                builder.Register(c=> c.Name("Test2").AuthorizedBy<DefaultAuthorizer>());
            });
        }
    }
}
